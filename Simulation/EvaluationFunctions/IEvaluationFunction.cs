using Simulation.SimResults;

namespace Simulation.EvaluationFunctions
{
    interface IEvaluationFunction
    {
        double Evaluate(SimulationResult results);
    }
}
