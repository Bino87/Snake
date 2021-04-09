using System;

namespace Network.Mutators
{
    enum MutationType
    {
        DuplicateNext,
        DuplicatePrevious,
        SwapWithNext,
        SwapWithPrevious,
        Randomize,
        Increment,
        Decrement,
    }

    public class BitMutator : IMutator
    {
        private readonly double _mutationChancePercentage;
        private readonly double _mutationPercentage;
        private readonly Random _rand;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mutationChancePercentage"> Set value from 0.0 to 1.0</param>
        /// <param name="mutationPercentage">Set value from 0.0 to 1.0</param>
        public BitMutator(double mutationChancePercentage, double mutationPercentage)
        {
            _mutationChancePercentage = mutationChancePercentage;
            _mutationPercentage = mutationPercentage;
            _rand = new Random();
        }

        public (NeuralNetwork First, NeuralNetwork Second) GetOffsprings(NeuralNetwork father, NeuralNetwork mother)
        {
            NetworkInfo fNetworkInfo = father.ToNetworkData();
            NetworkInfo mNetworkInfo = mother.ToNetworkData();

            byte[] fBytes = fNetworkInfo.ToByteArr();
            byte[] mBytes = mNetworkInfo.ToByteArr();

            byte[] first = MixBytes(fBytes, mBytes);
            byte[] second = MixBytes(fBytes, mBytes);

            Mutate(first);
            Mutate(second);

            fNetworkInfo.FromByteArray(first);
            mNetworkInfo.FromByteArray(second);

            

            return (new NeuralNetwork(fNetworkInfo), new NeuralNetwork(mNetworkInfo));
        }

        void Mutate(byte[] arr)
        {
            if (_rand.NextDouble() < _mutationChancePercentage)
            {
                int mutationAmount = (int) (_mutationPercentage * arr.Length);

                for(int i = 0; i < mutationAmount; i++)
                {
                    int index = _rand.Next(arr.Length);
                    Mutate(arr, index);
                }
            }
        }

        private void Mutate(byte[] arr, int index)
        {
            MutationType[] enums = Enum.GetValues<MutationType>();

            MutationType current = (MutationType) _rand.Next(enums.Length);

            switch(current)
            {

                case MutationType.DuplicateNext:
                    if (index + 1 < arr.Length)
                    {
                        arr[index] = arr[index + 1];
                    }
                    break;
                case MutationType.DuplicatePrevious: 
                    if (index - 1 >= 0)
                    {
                        arr[index] = arr[index - 1];
                    }
                    break;
                case MutationType.SwapWithNext:
                    if (index + 1 < arr.Length)
                    {
                        byte temp = arr[index];
                        arr[index] = arr[index + 1];
                        arr[index + 1] = temp;
                    }
                    break;
                case MutationType.SwapWithPrevious: 
                    if (index - 1 >= 0)
                    {
                        byte temp = arr[index];
                        arr[index] = arr[index - 1];
                        arr[index - 1] = temp;
                    }
                    break;
                case MutationType.Randomize:

                    arr[index] = (byte) _rand.Next(0, byte.MaxValue + 1);
                    break;
                case MutationType.Increment:
                    arr[index]++;
                    break;
                case MutationType.Decrement:
                    arr[index]--;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private  byte[] MixBytes(byte[] fBytes, byte[] mBytes)
        {

            byte[] bytes = new byte[fBytes.Length];

            (int fLen, int mLen) = (fBytes.Length / 2, mBytes.Length / 2);

            int len = fBytes.Length;

            for (int i = 0; i < len; i++)
            {
                int a = _rand.Next(0, fLen + mLen);

                if (a < fLen)
                {
                    bytes[i] = mBytes[i];
                    fLen--;
                }
                else
                {
                    bytes[i] = fBytes[i];
                    mLen--;
                }
            }

            return bytes;
        }
    }
}