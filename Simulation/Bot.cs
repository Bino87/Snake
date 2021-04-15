using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Network;
using Simulation.Core;
using Simulation.Enums;
using Simulation.Extensions;
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
        private SnakeHead Head => _snake[0] as SnakeHead;
        private SnakePart Tail => _snake[^1];
        private readonly Dictionary<(int, int), MapCellType> _takenCells;

        private readonly NetworkAgent _networkAgent;
        public int Generation { get; }
        public int ID { get; }
        private static int idCounter;
        private readonly List<HashSet<int>> _uniqueCells;
        public Bot(int mapSize, int maxMovesWithoutFood, NetworkInfo networkInfo, int generation)
        {
            _takenCells = new Dictionary<(int, int), MapCellType>();
            Generation = generation;
            ID = idCounter++;
            _mapSize = mapSize;
            _maxMovesWithoutFood = maxMovesWithoutFood;
            _rand = new Random();
            _snake = new List<SnakePart>();
            _networkAgent = new NetworkAgent(networkInfo);
            _uniqueCells = new List<HashSet<int>>();
        }

        public SimulationResult Run(Action<List<(int X, int Y, MapCellType Status)>, double[][], VisionData[]> onCellsUpdated)
        {

            List<(int X, int Y, MapCellType Status)> updateCellData = new();
            List<VisionData> visionData = new();
            _uniqueCells.Clear();
            _uniqueCells.Add(new HashSet<int>());
            _takenCells.Clear();

            SpawnSnake(updateCellData);
            SpawnNewFood(updateCellData);

            int movesSinceLastFood = 0;
            List<int> list = new();
            MovePrognosis movePrognosis;
            while (movesSinceLastFood < _maxMovesWithoutFood + _snake.Count)
            {
                (double[][] calculationResults, Direction direction) = CalculateSnakeDirection(visionData);

                updateCellData.Add((_food.X, _food.Y, _food.Type));
                visionData.Sort();
                onCellsUpdated?.Invoke(updateCellData, calculationResults, visionData.ToArray());
                updateCellData.Clear();
                visionData.Clear();

                movePrognosis = GetMoveResults(direction);

                if (movePrognosis == MovePrognosis.Ok)
                {
                    Move(ref movesSinceLastFood, list, updateCellData, direction);
                }
                else
                    break;

                movesSinceLastFood++;
                _uniqueCells[^1].Add(Head.Y * _mapSize + Head.X);
            }

            list.Add(movesSinceLastFood);
            return new SimulationResult(_snake.Count - cInitialSnakeSize, Generation, _uniqueCells);
        }

        public NeuralNetwork GetNeuralNetwork() => _networkAgent.GetNeuralNetwork();

        private void SpawnSnake(ICollection<(int X, int Y, MapCellType Status)> updateCellDatas)
        {
            _snake.Clear();
            int x = _mapSize / 2;

            for (int i = 0; i < cInitialSnakeSize && i < _mapSize && x + i < _mapSize; i++)
            {
                int y = x + i;
                _snake.Add(i == 0 ? new SnakeHead(x, y, Direction.Up) : new SnakePart(x, y, Direction.Up));
                updateCellDatas.Add((x, y, i == 0 ? MapCellType.Head : MapCellType.Snake));
                _takenCells.Add((x, y), _snake[^1].Type);
            }
        }

        private void SpawnNewFood(ICollection<(int X, int Y, MapCellType Status)> updateCellData)
        {
            int nextX = _rand.Next(0, _mapSize);
            int nextY = _rand.Next(0, _mapSize);

            while (_takenCells.ContainsKey((nextX, nextY)))
            {
                nextX = _rand.Next(0, _mapSize);
                nextY = _rand.Next(0, _mapSize);
            }

            _takenCells.Add((nextX, nextY), MapCellType.Food);
            _food = new Food(nextX, nextY);
            updateCellData.Add((nextX, nextY, MapCellType.Food));

        }

        private (double[][] calculationResults, Direction direction) CalculateSnakeDirection(List<VisionData> visionData)
        {
            double[][] results = _networkAgent.Calculate(new NetworkAgentCalculationParameters(Head, Tail, _food, _takenCells, _mapSize), visionData);
            return (results, PickBest(results[^1]));
        }

        private Direction PickBest(IReadOnlyList<double> result)
        {
            double max = double.MinValue;
            int index = 0;

            for (int i = 0; i < result.Count; i++)
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

        private void Move(ref int movesSinceLastFood, ICollection<int> list, List<(int X, int Y, MapCellType Status)> updateCellData, Direction direction)
        {
            (int x, int y) = Head.GetPositionAfterMove(direction);

            if (x == _food.X && y == _food.Y)
            {
                list.Add(movesSinceLastFood);
                EatFood(updateCellData);
                movesSinceLastFood = 0;
                _uniqueCells.Add(new HashSet<int>());
            }
            else
            {
                for (int i = 0; i < _snake.Count; i++)
                {
                    SnakePart snakePart = _snake[i];
                    Direction previous = snakePart.Direction;           //save previous dir
                    _takenCells.Remove((snakePart.X, snakePart.Y));     //remove cell from register

                    (int X, int Y) moveCoordinates = snakePart.Move(direction);
                    if (_takenCells.ContainsKey(moveCoordinates))
                        _takenCells.Remove(moveCoordinates);
                    _takenCells.Add(moveCoordinates, snakePart.Type);

                    updateCellData.Add((snakePart.X, snakePart.Y, snakePart.Type));

                    direction = previous;
                }
            }


        }

        private void EatFood(List<(int X, int Y, MapCellType Status)> updateCellData)
        {

            SnakePart[] arr = new SnakePart[_snake.Count + 1];
            arr[0] = new SnakeHead(_food.X, _food.Y, Head.Direction);       //spawn head where food is

            for (int i = 1; i < arr.Length; i++)
            {
                SnakePart temp = _snake[i - 1];
                arr[i] = new SnakePart(temp.X, temp.Y, temp.Direction);     //cache snake
            }

            _snake.Clear();                                                 //clear snake
            updateCellData.Clear();                                         //clear cell data
            _takenCells.Clear();                                            //clear taken cells

            for (int i = 0; i < arr.Length; i++)                            //rebuild snake
            {
                _snake.Add(arr[i]);
                updateCellData.Add((arr[i].X, arr[i].Y, arr[i].Type));       //rebuild cell data
                _takenCells.Add((arr[i].X, arr[i].Y), arr[i].Type);           //rebuild taken cells
            }

            SpawnNewFood(updateCellData);                                   //spawn food
        }

        private MovePrognosis GetMoveResults(Direction direction)
        {
            (int x, int y) = Head.GetPositionAfterMove(direction);

            if (x < 0 || y < 0 || x >= _mapSize || y >= _mapSize)
                return MovePrognosis.OutOfBounds;

            if (_takenCells.TryGetValue((x, y), out MapCellType item) && item != MapCellType.Food)
                return MovePrognosis.SelfCollision;


            return MovePrognosis.Ok;
        }
    }
}