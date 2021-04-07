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

        public int Move(int mapSize)
        {
            return Direction switch
                {
                    Direction.North => InternalIndex - mapSize,
                    Direction.East => InternalIndex - 1,
                    Direction.South => InternalIndex + mapSize,
                    Direction.West => InternalIndex + 1,
                    _ => throw new ArgumentOutOfRangeException()
                };
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