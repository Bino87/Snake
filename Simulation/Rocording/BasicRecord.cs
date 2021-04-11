using System;

namespace Simulation.Rocording
{
    public class BasicRecord : IEvaluationRecord<double[]>
    {

        public double[] this[Index index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        public double[] Last => Data[^1];

        public double[][] Data { get; }

        public BasicRecord(double[][] data)
        {
            Data = data;
        }

       
    }
}