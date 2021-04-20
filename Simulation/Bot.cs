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

    [DebuggerDisplay("{Id} : {Generation}")]
    public class Bot
    {
        private const int cInitialSnakeSize = 4;

        private readonly int _mapSize;
        private readonly List<SnakePart> _snake;
        private Food _food;
        private readonly int _maxMovesWithoutFood;
        private readonly Random _rand;
        private SnakeHead Head => _snake[0] as SnakeHead;
        private SnakePart Tail => _snake[^1];
        private readonly Dictionary<(int, int), MapCellType> _takenCells;

        private readonly NetworkAgent _networkAgent;
        public int Generation { get; }
        public int Id { get; }
        private static int _idCounter;
        private readonly List<HashSet<int>> _uniqueCells;
        public Bot(int mapSize, int maxMovesWithoutFood, NetworkInfo networkInfo, int generation)
        {
            _takenCells = new Dictionary<(int, int), MapCellType>();
            Generation = generation;
            Id = _idCounter++;
            _mapSize = mapSize;
            _maxMovesWithoutFood = maxMovesWithoutFood;
            _rand = new Random();
            _snake = new List<SnakePart>();
            _networkAgent = new NetworkAgent(networkInfo);
            _uniqueCells = new List<HashSet<int>>();
        }

        public SimulationResult Run(IUpdate<IOnMoveUpdateParameters> updater)
        {

            _uniqueCells.Clear();
            _uniqueCells.Add(new HashSet<int>());
            _takenCells.Clear();
            Dictionary<TurnDirection, double> turnDirections = new()
            {
                { TurnDirection.Right, 0 },
                { TurnDirection.None, 0 },
                { TurnDirection.Left, 0 },

            };
            SpawnSnake(updater);
            SpawnNewFood(updater);
            updater.Update();
            int movesSinceLastFood = 0;

            while (movesSinceLastFood < _maxMovesWithoutFood + _snake.Count)
            {
                (double[][] calculationResults, TurnDirection turnDirection) = CalculateSnakeDirection(updater);


                Update(updater, calculationResults, movesSinceLastFood);

                turnDirections[turnDirection]++;

                Direction direction = Head.Direction.Turn(turnDirection);

                MovePrognosis movePrognosis = GetMoveResults(direction);

                if (movePrognosis == MovePrognosis.Ok)
                {
                    movesSinceLastFood = Move(movesSinceLastFood, updater, direction);
                }
                else
                    break;

                _uniqueCells[^1].Add(Head.Y * _mapSize + Head.X);
            }

            double right = turnDirections[TurnDirection.Right];
            double left = turnDirections[TurnDirection.Left];

            double ratio = Math.Max(1, Math.Min(right, left)) / Math.Max(1, Math.Max(right, left));

            return new SimulationResult(Generation, _uniqueCells, _maxMovesWithoutFood, ratio);
        }

        private void Update(IUpdate<IOnMoveUpdateParameters> updater, IReadOnlyList<double[]> calculationResults, int movesSinceLastFood)
        {
            if (!updater.ShouldUpdate)
                return;
            updater.Data.Moves = movesSinceLastFood;
            updater.Data.Points = _uniqueCells.Count - 1;

            updater.Data.CellUpdateData.Add(new CellUpdateData(_food.X, _food.Y, _food.Type));

            for (int i = 0; i < calculationResults.Count; i++)
            {
                updater.Data.CalculationResults.Add(calculationResults[i]);
            }

            updater.Update();
        }

        public NeuralNetwork GetNeuralNetwork() => _networkAgent.GetNeuralNetwork();

        private void SpawnSnake(IUpdate<IOnMoveUpdateParameters> updater)
        {
            _snake.Clear();
            int x = _mapSize / 2;

            for (int i = 0; i < cInitialSnakeSize && i < _mapSize && x + i < _mapSize; i++)
            {
                int y = x + i;
                _snake.Add(i == 0 ? new SnakeHead(x, y, Direction.Up) : new SnakePart(x, y, Direction.Up));
                if (updater.ShouldUpdate)
                    updater.Data.CellUpdateData.Add(new CellUpdateData(x, y, i == 0 ? MapCellType.Head : MapCellType.Snake));
                _takenCells.Add((x, y), _snake[^1].Type);
            }
        }

        private void SpawnNewFood(IUpdate<IOnMoveUpdateParameters> updater)
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
            if (updater.ShouldUpdate)
                updater.Data.CellUpdateData.Add(new CellUpdateData(nextX, nextY, MapCellType.Food));

        }

        private (double[][] calculationResults, TurnDirection direction) CalculateSnakeDirection(IUpdate<IOnMoveUpdateParameters> updater)
        {
            double[][] results = _networkAgent.Calculate(new NetworkAgentCalculationParameters(Head, Tail, _food, _takenCells, _mapSize), updater);
            return (results, PickBest(results[^1]));
        }

        private TurnDirection PickBest(IReadOnlyList<double> result)
        {
            double max = double.MinValue;
            int index = 0;

            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] == 0)
                    continue;
                if (max > result[i])
                    continue;

                if (max <= result[i])
                {
                    index = i;
                    max = result[i];
                }

                if (max == result[i])
                    index = i;
            }

            return index switch
            {
                0 => TurnDirection.Left,
                1 => TurnDirection.None,
                2 => TurnDirection.Right,

                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private int Move(int movesSinceLastFood, IUpdate<IOnMoveUpdateParameters> updater, Direction direction)
        {
            (int x, int y) = Head.GetPositionAfterMove(direction);

            if (x == _food.X && y == _food.Y)
            {
                EatFood(updater, direction);
                return 0;
            }
            else
            {
                Move(updater, direction);
                return movesSinceLastFood + 1;
            }
        }

        private void Move(IUpdate<IOnMoveUpdateParameters> updater, Direction direction)
        {

            for (int i = 0; i < _snake.Count; i++)
            {
                SnakePart snakePart = _snake[i];
                Direction previous = snakePart.Direction; //save previous dir
                _takenCells.Remove((snakePart.X, snakePart.Y)); //remove cell from register

                (int X, int Y) moveCoordinates = snakePart.Move(direction);
                if (_takenCells.ContainsKey(moveCoordinates))
                    _takenCells.Remove(moveCoordinates); //seams like there is a bug here somewhere
                _takenCells.Add(moveCoordinates, snakePart.Type);

                if (updater.ShouldUpdate)
                    updater.Data.CellUpdateData.Add(new CellUpdateData(snakePart.X, snakePart.Y, snakePart.Type));

                direction = previous;
            }
        }


        private void EatFood(IUpdate<IOnMoveUpdateParameters> updater, Direction direction)
        {
            int x = Tail.X;
            int y = Tail.Y;
            Direction d = Tail.Direction;

            _takenCells.Remove((_food.X, _food.Y));
            Move(updater, direction);
            _snake.Add(new SnakePart(x, y, d));
            _takenCells.Add((x, y), MapCellType.Snake);

            _uniqueCells.Add(new HashSet<int>());
            SpawnNewFood(updater);
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