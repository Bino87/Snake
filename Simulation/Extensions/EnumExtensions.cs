using System;
using Network.Mutators;
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

        public static (int X, int Y) GetMove(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => (0, -1),
                Direction.Right => (1, 0),
                Direction.Down => (0, 1),
                Direction.Left => (-1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static IMutator GetMutator(this MutationTechnique technique, double mutationChance, double mutationRate)
        {
            return technique switch
            {
                MutationTechnique.String => new StringMutator(mutationChance, mutationRate, 2,12),
                MutationTechnique.BitArr => new BitMutator(mutationChance, mutationRate),
                _ => throw new ArgumentOutOfRangeException()

            };
        }
    }
}
