using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using Network;
using Network.ActivationFunctions;
using Simulation.Core;
using Simulation.Enums;
using Simulation.Extensions;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation
{
    [DebuggerDisplay("{ID}")]
    public class Bot
    {
        private readonly int _mapSize;
        private readonly List<SnakePart> _snake;
        private Food _food;
        private readonly int _maxMovesWithoutFood;
        private readonly Random _rand;
        private readonly IMapCell[,] _map;
        private SnakePart Head => _snake[0];
        private SnakePart Tail => _snake[^1];

        private readonly NetworkAgent _networkAgent;
        public int ID { get; }
        private static int idCounter;
        public Bot(IMapCell[,] map, int mapSize, int maxMovesWithoutFood, NetworkInfo networkInfo)
        {
            ID = idCounter++;
            _mapSize = mapSize;
            _map = map;
            _maxMovesWithoutFood = maxMovesWithoutFood;
            _rand = new Random();
            _snake = new List<SnakePart>();
            _networkAgent = new NetworkAgent(map, networkInfo);
        }

        public NeuralNetwork GetNeuralNetwork() => _networkAgent.GetNeuralNetwork();

        private void SpawnSnake(int snakeSize, Action<int, int, MapCellStatus> callback)
        {
            _snake.Clear();
            int x = _mapSize / 2;

            for (int i = 0; i < snakeSize && i < _mapSize; i++)
            {
                int y = x + i;
                _snake.Add(new SnakePart(x, y, Direction.Up));
                callback?.Invoke(x, y, MapCellStatus.Snake);
            }
        }

        private void SpawnNewFood(Action<int, int, MapCellStatus> callback)
        {
            int nextX = _rand.Next(0, _mapSize);
            int nextY = _rand.Next(0, _mapSize);

            while (_map[nextX, nextY].CellStatus != MapCellStatus.Empty)
            {
                nextX = _rand.Next(0, _mapSize);
                nextY = _rand.Next(0, _mapSize);
            }

            _food = new Food(nextX, nextY);
            callback?.Invoke(nextX, nextY, MapCellStatus.Food);

        }

        private void CalculateSnakeDirection(Action<double[][]> netCallback)
        {
            double[][] results = _networkAgent.Calculate(Head, Tail.Direction, _mapSize, _food.X, _food.Y);
            Head.Direction = PickBest(results[^1]);
            netCallback?.Invoke(results);
        }

        private Direction PickBest(double[] result)
        {
            double max = double.MinValue;
            int index = 0;

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == 0)
                    continue;
                if (max > result[i])
                    continue;

                if (max < result[i])
                {
                    index = i;
                    max = result[i];
                }

                if (max == result[i])
                    index = i;
            }

            TurnDirection td = (TurnDirection) index;

            return Head.Direction.Turn(td);
        }

        private void Move(Action<int, int, MapCellStatus> callback, ref int movesSinceLastFood, List<int> list)
        {
            (int x, int y) = Head.GetMove();
            x = Head.X + x;
            y = Head.Y + y;
            if (x == _food.X && y == _food.Y)
            {
                list.Add(movesSinceLastFood);
                EatFood(callback);
                movesSinceLastFood = 0;
            }
            else
            {
                Direction dir = Head.Direction;
                Head.Move();
                callback(Head.X, Head.Y, MapCellStatus.Snake);
                (x, y) = (Tail.X, Tail.Y);
                callback(x, y, MapCellStatus.Empty);

                for (int i = 1; i < _snake.Count; i++)
                {
                    Direction tDir = _snake[i].Direction;
                    _snake[i].Move();
                    _snake[i].Direction = dir; //sets the direction to that of an previous element, head is getting it's own direction.
                    dir = tDir;
                }
            }
        }

        private void EatFood(Action<int, int, MapCellStatus> callback)
        {
            _snake.Insert(0, new SnakePart(_food.X, _food.Y, Head.Direction));
            callback?.Invoke(Head.X, Head.Y, MapCellStatus.Snake);

            SpawnNewFood(callback);
        }

        public SimulationResult Run([NotNull] Action<int, int, MapCellStatus> callback, Action<double[][]> netCallback)
        {
            ResetMap(callback);
            SpawnSnake(4, callback);
            SpawnNewFood(callback);
            MovePrognosis movePrognosis = MovePrognosis.Ok;

            int movesSinceLastFood = 0;
            List<int> list = new();

            while (movesSinceLastFood < _maxMovesWithoutFood)
            {
                CalculateSnakeDirection(netCallback);

                movePrognosis = GetMoveResults();

                if (movePrognosis == MovePrognosis.Ok)
                {
                    Move(callback, ref movesSinceLastFood, list);
                }
                else
                {
                    break;
                }

                movesSinceLastFood++;
            }

            list.Add(movesSinceLastFood);
            return new SimulationResult(_snake.Count - 4, list, movePrognosis == MovePrognosis.SelfCollision, movePrognosis == MovePrognosis.OutOfBounds);
        }

        private void ResetMap(Action<int, int, MapCellStatus> callback)
        {
            for (int x = 0; x < _mapSize; x++)
            {
                for (int y = 0; y < _mapSize; y++)
                {
                    if (_map[x, y].CellStatus != MapCellStatus.Empty)
                        callback?.Invoke(x, y, MapCellStatus.Empty);
                }
            }
        }

        private MovePrognosis GetMoveResults()
        {
            if (!Head.IsValidMove(_map))//checks if it leaves the bounds of map
                return MovePrognosis.OutOfBounds;

            //calculate this shit after you move you cunt
            (int x, int y) = GetPositionAfterMove();

            if (_map[x, y].CellStatus == MapCellStatus.Snake)
                return MovePrognosis.SelfCollision;

            return MovePrognosis.Ok;
        }

        private (int X, int Y) GetPositionAfterMove()
        {
            (int x, int y) = Head.GetMove();

            return (x + Head.X, y + Head.Y);
        }
    }
}