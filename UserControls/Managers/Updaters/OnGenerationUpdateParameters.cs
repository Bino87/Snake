using Simulation.Interfaces;

namespace UserControls.Managers.Updaters
{
    public class OnGenerationUpdateParameters : IOnGenerationUpdateParameters
    {
        public int Generation { get; set; }
        public double AverageFitnessValue { get; set; }
    }
}