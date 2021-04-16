using Simulation.Enums;

namespace Simulation.Interfaces
{
    public interface ISimulationInitParameters
    {
        int MapSize { get; set; }
        int NumberOfPairs { get; set; }
        int UpdateDelay { get; set; }
        bool RunInBackground { get; set; }
        MutationTechnique MutationTechnique { get; set; }
        double MutationChance { get; set; }
        double MutationRate { get; set; }
        int MaxMoves { get; set; }
    }
}