using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Simulation.Core;
using Simulation.Enums;

namespace Simulation
{
    public class MapManager
    {
        private readonly int _mapSize;
        private readonly List<SnakePart> _snake;
        private Food _food;
        private readonly HashSet<int> _occupiedTiles;
        private readonly int _maxMovesWithoutFood;
        private Random _rand;
        private SnakePart Head => _snake[0];
        private SnakePart Tail => _snake[^1];

        private NetworkAgent na;

        public MapManager(int mapSize, int maxMovesWithoutFood, Action<int, MapCellStatus> callback)
        {
            _mapSize = mapSize;
            _maxMovesWithoutFood = maxMovesWithoutFood;
            _rand = new Random();
            _snake = new List<SnakePart>();
            _occupiedTiles = new HashSet<int>();
            na = new NetworkAgent(0.5, 2 * 4 + 8 * 3, 30,20, 10, 4);

            int startIndex = mapSize * mapSize / 2;

            _snake.Add(new SnakePart(startIndex, Direction.North));
            _snake.Add(new SnakePart(startIndex - mapSize, Direction.North));
            _snake.Add(new SnakePart(startIndex - mapSize *2, Direction.North));
            _snake.Add(new SnakePart(startIndex - mapSize *3, Direction.North));
            for(int i = 0; i < 25; i++)
                SpawnNewFood();
            callback(_food.InternalIndex, MapCellStatus.Food);
        }

        private void SetSnakeDirection()
        {
            Head.Direction = na.Calculate(_occupiedTiles, Head, _food, Tail.Direction, _mapSize);
        }

        private void EatFood(Action<int, MapCellStatus> callback)
        {
            _snake.Insert(0, new SnakePart(_food.InternalIndex, Head.Direction));
            callback(_food.InternalIndex, MapCellStatus.Snake);

            SpawnNewFood();
            callback(_food.InternalIndex, MapCellStatus.Food);
        }

        private void Move(Action<int, MapCellStatus> callback, ref int movesSinceLastFood, List<int> list)
        {
            //if eat food than do something else
            if (Head.Move(_mapSize) == _food.InternalIndex)
            {
                list.Add(movesSinceLastFood);
                EatFood(callback);
                movesSinceLastFood = 0;
                return;
            }

            _occupiedTiles.Remove(Tail.InternalIndex);
            callback(Tail.InternalIndex, MapCellStatus.Empty);

            Direction dir = Head.Direction;
            Head.InternalIndex = Head.Move(_mapSize);
            _occupiedTiles.Add(Head.InternalIndex);
            //_occupiedTiles.Add(Head.InternalIndex);
            callback(Head.InternalIndex, MapCellStatus.Snake);

            for (int i = 1; i < _snake.Count; i++)
            {
                _snake[i].Direction = dir; //sets the direction to that of an previous element, head is getting it's own direction.

                _snake[i].InternalIndex = _snake[i].Move(_mapSize);
            }
        }



        public SimulationResult Run([NotNull] Action<int, MapCellStatus> callback)
        {
            int movesSinceLastFood = 0;
            List<int> list = new List<int>();
            bool shouldRun = true;
            Move(callback, ref movesSinceLastFood, list);
            while (movesSinceLastFood < _maxMovesWithoutFood)
            {
                SetSnakeDirection();

                MovePrognosis movePrognosis = GetMoveResults();

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

            return new SimulationResult(_snake.Count, list);
        }

       
        private void SpawnNewFood()
        {
            int min = 0;
            int max = _mapSize * _mapSize;
            int nextFoodIndex = _rand.Next(min, max);

            while (_occupiedTiles.Contains(nextFoodIndex))
            {
                if (nextFoodIndex == min) min++;
                if (nextFoodIndex == max) max--;

                nextFoodIndex = _rand.Next(min, max);
            }

            _food = new Food(nextFoodIndex);
        }

        private MovePrognosis GetMoveResults()
        {
            if (!Head.IsValidMove(_mapSize, _mapSize * _mapSize))//checks if it leaves the bounds of map
                return MovePrognosis.OutOfBounds;


            //calculate this shit after you move you cunt
            if (_occupiedTiles.Contains(GetIndexAfterMove()))
                return MovePrognosis.SelfCollision;

            return MovePrognosis.Ok;

        }

        int GetIndexAfterMove()
        {
            int CalculateMove()
            {
                return Head.Direction switch
                    {
                        Direction.North => -_mapSize,
                        Direction.East => 1,
                        Direction.South => _mapSize,
                        Direction.West => -1,
                        _ => throw new ArgumentOutOfRangeException()
                    };
            }

            return CalculateMove() + Head.InternalIndex;
        }
    }
}
