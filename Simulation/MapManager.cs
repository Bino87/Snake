using System;
using System.Collections.Generic;
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



        public MapManager(int mapSize, int maxMovesWithoutFood)
        {
            _mapSize = mapSize;
            _maxMovesWithoutFood = maxMovesWithoutFood;
            _rand = new Random();
            _snake = new List<SnakePart>();
            _occupiedTiles = new HashSet<int>();
        }

        private void SetSnakeDirection(Direction current)
        {
            Head.Direction = current;
        }

        private void EatFood(Action<int, MapCellStatus> callback)
        {
            _snake.Insert(0, new SnakePart(_food.InternalIndex, Head.Direction));
            callback(_food.InternalIndex, MapCellStatus.Snake);

            SpawnNewFood();
            callback(_food.InternalIndex, MapCellStatus.Food);
        }

        private void Move(Action<int, MapCellStatus> callback)
        {
            _occupiedTiles.Remove(Tail.InternalIndex);
            callback(Tail.InternalIndex, MapCellStatus.Empty);

            Direction dir = Head.Direction;
            Head.Move(_mapSize);
            _occupiedTiles.Add(Head.InternalIndex);
            callback(Head.InternalIndex, MapCellStatus.Snake);

            for (int i = 1; i < _snake.Count; i++)
            {
                _snake[i].Direction = dir; //sets the direction to that of an previous element, head is getting it's own direction.

                _snake[i].Move(_mapSize);
            }
        }

        public SimulationResult Run([NotNull] Action<int, MapCellStatus> callback)
        {
            int movesSinceLastFood = 0;
            List<int> list = new List<int>();
            bool shouldRun = true;

            while (shouldRun)
            {
                SetSnakeDirection(Head.Direction);

                switch (GetMoveResults())
                {
                    case MoveResults.Ok:
                        Move(callback);
                        break;
                    case MoveResults.EatFood:
                        list.Add(movesSinceLastFood);
                        EatFood(callback);
                        movesSinceLastFood = 0;
                        break;
                    case MoveResults.OutOfBounds:
                    case MoveResults.SelfCollision:
                        //needs some more things to happen
                        shouldRun = false;
                        break;

                    default: throw new ArgumentOutOfRangeException();
                }

                movesSinceLastFood++;
                shouldRun = shouldRun && movesSinceLastFood < _maxMovesWithoutFood;
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

        private MoveResults GetMoveResults()
        {
            if (!Head.IsValidMove(_mapSize, _snake.Count))//checks if it leaves the bounds of map
                return MoveResults.OutOfBounds;

            if (_occupiedTiles.Contains(Head.InternalIndex))
                return MoveResults.SelfCollision;

            if (_occupiedTiles.Contains(_food.InternalIndex))
                return MoveResults.EatFood;


            return MoveResults.Ok;

        }
    }
}
