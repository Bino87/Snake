using System;

namespace Simulation.Rocording
{
    internal interface IEvaluationRecord<T>
    {
        T this[Index index] { get; set; }
        T[] Data { get; }
    }
}