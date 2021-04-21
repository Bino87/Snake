using System.Collections.Generic;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation.Extensions
{
    public static class Extensions
    {
        public static double CalculateAverageResult(this IReadOnlyList<FitnessResults> results)
        {
            double total = 0;
            var len = results.Count / 2;

            for (int i = 0; i < len; i++)
            {
                total += results[i].Result.Points;
            }

            total /= results.Count;
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