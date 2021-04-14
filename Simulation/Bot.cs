using System;
using System.Collections.Generic;
using System.Diagnostics;
using Network;
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
        private const int cInitialSnakeSize = 3;

        private readonly int _mapSize;
        private readonly List<SnakePart> _snake;
        private Food _food;
        private readonly int _maxMovesWithoutFood;
        private Random _rand;
        private readonly IMapCell[,] _map;
        private SnakePart Head => _snake[0];
        private SnakePart Tail => _snake[^1];

        private readonly NetworkAgent _networkAgent;
        public int Generation { get; }
        public int ID { get; }
        private static int idCounter;
        public Bot(IMapCell[,] map, int mapSize, int maxMovesWithoutFood, NetworkInfo networkInfo, int generation)
        {
            Generation = generation;
            ID = idCounter++;
            _mapSize = mapSize;
            _map = map;
            _maxMovesWithoutFood = maxMovesWithoutFood;

            _snake = new List<SnakePart>();
            _networkAgent = new NetworkAgent(map, networkInfo);
        }

        public SimulationResult Run(Action<List<(int X, int Y, MapCellStatus Status)>, double[][],VisionData[]> onCellsUpdated)
        {
            _rand = new Random(1);
            List<(int X, int Y, MapCellStatus Status)> updateCellData = new();
            List<VisionData> visionData = new();
            ResetMap(updateCellData);
            SpawnSnake(updateCellData);
            SpawnNewFood(updateCellData);

            int movesSinceLastFood = 0;
            List<int> list = new();
            MovePrognosis movePrognosis;
            while (movesSinceLastFood < _maxMovesWithoutFood + _snake.Count)
            {
                double[][] values = CalculateSnakeDirection(visionData);

                visionData.Sort();
                movePrognosis = GetMoveResults();
                onCellsUpdated?.Invoke(updateCellData, values, visionData.ToArray());
                visionData.Clear();

                

                if (movePrognosis == MovePrognosis.Ok)
                {
                    Move(ref movesSinceLastFood, list, updateCellData);
                }
                else
                    break;

                movesSinceLastFood++;
               
            }

            list.Add(movesSinceLastFood);
            return new SimulationResult(_snake.Count - cInitialSnakeSize, list, Generation);
        }

        public NeuralNetwork GetNeuralNetwork() => _networkAgent.GetNeuralNetwork();

        private void SpawnSnake(ICollection<(int X, int Y, MapCellStatus Status)> updateCellDatas)
        {
            _snake.Clear();
            int x = _mapSize / 2;
            int y = 0;

            for (int i = 0; i < cInitialSnakeSize && i < _mapSize && x + i < _mapSize; i++)
            {
                y = x + i;
                _snake.Add(new SnakePart(x, y, Direction.Up));
                updateCellDatas.Add((x, y, i == 0 ? MapCellStatus.Head : MapCellStatus.Snake));
            }
        }

        private void SpawnNewFood(List<(int X, int Y, MapCellStatus Status)> updateCellData)
        {
            int nextX = _rand.Next(0, _mapSize);
            int nextY = _rand.Next(0, _mapSize);

            while (_map[nextX, nextY].CellStatus != MapCellStatus.Empty)
            {
                nextX = _rand.Next(0, _mapSize);
                nextY = _rand.Next(0, _mapSize);
            }

            _food = new Food(nextX, nextY);
            updateCellData.Add((nextX, nextY, MapCellStatus.Food));

        }

        private double[][] CalculateSnakeDirection(List<VisionData> visionData)
        {
            double[][] results = _networkAgent.Calculate(Head.Direction, Tail.Direction, _mapSize, _food.X, _food.Y, visionData);
            Head.Direction = PickBest(results[^1]);
            return results;
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

            TurnDirection td = (TurnDirection)index;

            return Head.Direction.Turn(td);
        }

        private void Move(ref int movesSinceLastFood, List<int> list, List<(int X, int Y, MapCellStatus Status)> updateCellData)
        {
            (int x, int y) = Head.GetMove();
            x = Head.X + x;
            y = Head.Y + y;
            if (x == _food.X && y == _food.Y)
            {
                list.Add(movesSinceLastFood);
                EatFood(updateCellData);
                movesSinceLastFood = 0;
            }
            else
            {
                Direction dir = Head.Direction;
                Head.Move();
                updateCellData.Add((Head.X, Head.Y, MapCellStatus.Head));
                
                (x, y) = (Tail.X, Tail.Y);
                updateCellData.Add((x, y, MapCellStatus.Empty));

                for (int i = 1; i < _snake.Count; i++)
                {
                    Direction tDir = _snake[i].Direction;
                    _snake[i].Move();
                    _snake[i].Direction = dir; //sets the direction to that of an previous element, head is getting it's own direction.
                    dir = tDir;
                }
            }
        }

        private void EatFood(List<(int X, int Y, MapCellStatus Status)> updateCellData)
        {
            _snake.Insert(0, new SnakePart(_food.X, _food.Y, Head.Direction));
            updateCellData.Add((Head.X, Head.Y, MapCellStatus.Snake));

            SpawnNewFood(updateCellData);
        }

        private void ResetMap(List<(int X, int Y, MapCellStatus Status)> updateCellData)
        {
            for (int x = 0; x < _mapSize; x++)
            {
                for (int y = 0; y < _mapSize; y++)
                {
                    if (_map[x, y].CellStatus != MapCellStatus.Empty)
                        updateCellData.Add((x, y, MapCellStatus.Empty));
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