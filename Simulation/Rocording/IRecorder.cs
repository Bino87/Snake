using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Rocording
{
    internal interface IRecorder<T>
    {
        int Layers { get; }
        void Record(double[] result);

        IEvaluationRecord<T> GetRecord();

    }
}
