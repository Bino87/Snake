namespace Simulation.Interfaces
{
    public interface ISimulationState
    {
        int Generation { get; set; }
        int Individual { get; set; }
        int Moves { get; set; }
        int Points { get; set; }

        // add some more stuff like whatever the fuck
    }
}