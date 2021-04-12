namespace Simulation.SimResults
{
    public readonly struct FitnessParameters
    {
        public FitnessParameters(double selfCollision, double outOfBounds, double medianFactor, double minFactor, double maxFactor, double avgFactor, double pointsFactor) : this()
        {
            SelfCollision = selfCollision;
            OutOfBounds = outOfBounds;
            MedianFactor = medianFactor;
            MinFactor = minFactor;
            MaxFactor = maxFactor;
            AvgFactor = avgFactor;
            PointsFactor = pointsFactor;
        }

        public double SelfCollision { get; }
        public double OutOfBounds { get; }
        public double MedianFactor { get; }
        public double MinFactor { get; }
        public double MaxFactor { get; }
        public double AvgFactor { get; }
        public double PointsFactor { get; }

    }
}