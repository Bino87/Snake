using System;
using Simulation.Enums;

namespace Simulation.Extensions
{
    public static class EnumExtensions
    {

        public static Direction Turn(this Direction direction, TurnDirection turn)
        {
            return (direction, turn) switch
            {
                (_, TurnDirection.None) => direction,
                (Direction.Up, TurnDirection.Left) => Direction.Left,
                (Direction.Up, TurnDirection.Right) => Direction.Right,

                (Direction.Left, TurnDirection.Left) => Direction.Down,
                (Direction.Left, TurnDirection.Right) => Direction.Up,

                (Direction.Down, TurnDirection.Left) => Direction.Right,
                (Direction.Down, TurnDirection.Right) => Direction.Left,

                (Direction.Right, TurnDirection.Left) => Direction.Up,
                (Direction.Right, TurnDirection.Right) => Direction.Down,

                _ => throw new Exception()
            };
        }
    }
}
