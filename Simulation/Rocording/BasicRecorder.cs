using System;

namespace Simulation.Rocording
{
    internal  class BasicRecorder : IRecorder<double[]>
    {
        private int currentIndex;
        private readonly IEvaluationRecord<double[]> _results;

        public BasicRecorder(int layers)
        {
            Layers = layers;
            _results = new BasicRecord(new double[Layers][]);
        }

        public int Layers { get; }
        public void Record(double[] result)
        {
            if (currentIndex >= Layers)
                throw new Exception();
            _results[currentIndex] = result;
            currentIndex++;

        }

        public IEvaluationRecord<double[]> GetRecord()
        {
            return _results;
        }
    }
}