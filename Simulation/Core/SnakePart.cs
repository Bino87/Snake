using System;
using Simulation.Enums;

namespace Simulation.Core
{
    public class SnakePart : SimObject
    {
        public override MapCellStatus Status => MapCellStatus.Snake;

        public Direction Direction { get; set; }
        public SnakePart(int internalIndex, Direction direction) : base(internalIndex)
        {
            Direction = direction;
        }

        public void Move(int mapSize)
        {
            switch(Direction)
            {
                case Direction.North:
                    InternalIndex -= mapSize; break;
                case Direction.East:
                    InternalIndex--;
                    break;
                case Direction.South:
                    InternalIndex += mapSize;
                    break;
                case Direction.West:
                    InternalIndex++;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsValidMove(int mapSize, int numTiles)
        {
            return Direction switch
            {
                Direction.North => InternalIndex - mapSize > 0,
                Direction.East => InternalIndex - 1 > 0 && (InternalIndex - 1) % mapSize < InternalIndex % mapSize,
                Direction.South => InternalIndex + mapSize < numTiles,
                Direction.West => InternalIndex + 1 < numTiles && (InternalIndex + 1) % mapSize > InternalIndex % mapSize,
                _ => throw new ArgumentOutOfRangeException(),
                };
        }
    }
}