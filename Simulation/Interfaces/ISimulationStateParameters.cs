using Simulation.Enums;

namespace Simulation.Interfaces
{
    public interface ISimulationStateParameters
    {
        int MapSize { get; set; }
        int NumberOfPairs { get; set; }
        int UpdateDelay { get; set; }
        bool RunInBackground { get; set; }
        MutationTechnique MutationTechnique { get; set; }
        double MutationChance { get; set; }
        double MutationRate { get; set; }
        int MaxMoves { get; }
        int CurrentIndividual { get; set; }

    }
}