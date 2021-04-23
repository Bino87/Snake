using System.Collections.Generic;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation.Extensions
{
    public static class Extensions
    {
        public static double CalculateAverageResultOfTopPercent(this IReadOnlyList<FitnessResults> results, double percentile)
        {
            double total = 0;
            int len = (int) (results.Count * percentile);

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
    }
}