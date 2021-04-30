using System.Collections.Generic;
using Simulation.Enums;

namespace Simulation.Core
{
    internal record NetworkAgentCalculationParameters(SnakeHead Head, SnakePart Tail, Food Food, Dictionary<(int, int), MapCellType> LookUp, int MapSize);

}