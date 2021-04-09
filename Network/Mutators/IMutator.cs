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
        private const byte cBitsInByte = 8;
        private const byte cOne = 1;
        public (NeuralNetwork First, NeuralNetwork Second) GetOffspring(NeuralNetwork father, NeuralNetwork mother)
        {
            NetworkInfo fNetworkInfo = father.ToNetworkData();
            NetworkInfo mNetworkInfo = mother.ToNetworkData();

            byte[] fBytes = fNetworkInfo.ToByteArr();
            byte[] mBytes = mNetworkInfo.ToByteArr();

            (byte[] first, byte[] second) = MixBytes(fBytes, mBytes);

            fNetworkInfo.FromByteArray(first);
            mNetworkInfo.FromByteArray(second);

            return (new NeuralNetwork(fNetworkInfo), new NeuralNetwork(mNetworkInfo));
        }

        private static bool GetBit(byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        private static (byte[] First, byte[] Second) MixBytes(byte[] fBytes, byte[] mBytes)
        {
            Random rand = new Random();

            byte[] first = new byte[fBytes.Length];
            byte[] second = new byte[fBytes.Length];

            (int fLen, int mLen) = (fBytes.Length / 2, mBytes.Length / 2);

            int len = fBytes.Length;

            for (int i = 0; i < len; i++)
            {
                int a = rand.Next(0, fLen + mLen);
                
                if (a < fLen)
                {
                    first[i] = mBytes[i];
                    second[i] = fBytes[i];
                    fLen--;
                    if (fLen < 0) fLen = 0;
                }
                else
                {
                    first[i] = fBytes[i];
                    second[i] = mBytes[i];
                    mLen--;
                    if (mLen < 0) mLen = 0;
                }
            }



            return (first, second);
        }
    }
}
