using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Mutators
{
    public interface IMutator
    {
        (NeuralNetwork First, NeuralNetwork Second) GetOffsprings(NeuralNetwork father, NeuralNetwork mother);
    }
}
