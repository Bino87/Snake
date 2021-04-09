using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Mutators
{
    public interface IMutator
    {
        (NeuralNetwork First, NeuralNetwork Second) GetOffspring(NeuralNetwork father, NeuralNetwork mother);
    }

    public class BitMutator : IMutator
    {
        public (NeuralNetwork First, NeuralNetwork Second) GetOffspring(NeuralNetwork father, NeuralNetwork mother)
        {
            byte[] fBytes = father.GetNetworkData().ToByteArr();
            byte[] mBytes = mother.GetNetworkData().ToByteArr();


            return (null, null);
        }
    }
}
