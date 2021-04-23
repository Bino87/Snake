using System.Collections.Generic;
using Simulation.Core;
using Simulation.Enums;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation.Extensions
{
    public static class Extensions
    {
        public static double CalculateAverageResultOfTopPercent(this IReadOnlyList<FitnessResults> results, double percentile)
        {
            double total = 0;
            int len = (int)(results.Count * percentile);

            for (int i = 0; i < len; i++)
            {
                total += results[i].Result.Points;
            }

            total /= len;
            return total;
        }

        public static void UpdateOnGeneration(this IUpdate<IOnGenerationUpdateParameters> onGeneration, double avg)
        {
            if (onGeneration is null)
                return;

            onGeneration.Data.AverageFitnessValue = avg;
            onGeneration.Data.Generation++;
            onGeneration.Update();
        }

        public static void TryAddCellUpdateData(this IUpdate<IOnMoveUpdateParameters> updater, int x, int y, MapCellType type)
        {
            if (updater is null)
                return;
            if (updater.ShouldUpdate)
                updater.Data.CellUpdateData.Add(new CellUpdateData(x, y, type));
        }

        public static void TryAddVisionData(this IUpdate<IOnMoveUpdateParameters> updater, int originX, int originY, int x, int y, VisionCollisionType type)
        {
            if (updater is null)
                return;
            if (updater.ShouldUpdate)
                updater.Data.VisionData.Add(new VisionData(type, originX, x, originY, y));
        }

        public static void UpdateVisual(this IUpdate<IOnMoveUpdateParameters> updater, IEnumerable<double[]> calculationResults, int movesSinceLastFood, Food food, IEnumerable<SnakePart> snakeParts, IList<HashSet<int>> uniqueCells)
        {
            if (updater is null)
                return;

            if (!updater.ShouldUpdate)
                return;

            updater.Data.Moves = movesSinceLastFood;
            updater.Data.Points = uniqueCells.Count - 1;
            updater.Data.CellUpdateData.Add(new CellUpdateData(food.X, food.Y, food.Type));

            foreach (SnakePart snakePart in snakeParts)
            {
                updater.Data.CellUpdateData.Add(new CellUpdateData(snakePart.X, snakePart.Y, snakePart.Type));
            }

            foreach (double[] t in calculationResults)
            {
                updater.Data.CalculationResults.Add(t);
            }

            updater.Update();
        }

        public static void UpdateIndividual(this ISimulationUpdateManager updateManager, double[][] weights, int index)
        {
            if (updateManager is null)
                return;

            if (updateManager.OnIndividual.ShouldUpdate)
            {
                foreach (double[] t in weights)
                {
                    updateManager.OnIndividual.Data.Weights.Add(t);
                }

                updateManager.OnIndividual.Data.IndividualIndex = index;
                updateManager.OnIndividual.Update();
            }
        }
    }
}