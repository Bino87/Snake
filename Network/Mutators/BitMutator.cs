using System;
using System.Data;

namespace Network.Mutators
{
    public class BitMutator : IMutator
    {
        private readonly double _mutationChancePercentage;
        private readonly double _mutationPercentage;
        private readonly Random _rand;
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


        private bool doStuff = false;
        public (NetworkInfo First, NetworkInfo Second) GetOffsprings(NeuralNetwork father, NeuralNetwork mother)
        {
            NetworkInfo fNetworkInfo = father.ToNetworkInfo();
            NetworkInfo mNetworkInfo = mother.ToNetworkInfo();

            byte[] fBytes = fNetworkInfo.ToByteArr();
            byte[] mBytes = mNetworkInfo.ToByteArr();

            byte[] first = MixBytes(fBytes, mBytes);
            byte[] second = MixBytes(fBytes, mBytes);

            first = Mutate(first);
            second = Mutate(second);

            fNetworkInfo.FromByteArray(first);
            mNetworkInfo.FromByteArray(second);

            if (doStuff)
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("father");
                dt.Columns.Add("mother");
                dt.Columns.Add("first");
                dt.Columns.Add("second");

                for (int i = 0; i < fBytes.Length; i++)
                {
                    dt.Rows.Add(
                        fBytes[i],
                        mBytes[i],
                        first[i],
                        second[i]
                    );
                }
            }


            return (fNetworkInfo, mNetworkInfo);
        }

        private byte[] Mutate(byte[] arr)
        {
            if (_rand.NextDouble() < _mutationChancePercentage)
            {
                int mutationAmount = (int)(_mutationPercentage * arr.Length);

                for (int i = 0; i < mutationAmount; i++)
                {
                    int index = _rand.Next(arr.Length);
                    Mutate(arr, index);
                }
            }

            return arr;
        }

        private void Mutate(byte[] arr, int index)
        {
            MutationType[] enums = Enum.GetValues<MutationType>();
            //enums = new[] { MutationType.Decrement, MutationType.Increment, /*MutationType.SwapWithNext,MutationType.SwapWithPrevious*/};

            MutationType current = (MutationType)_rand.Next(enums.Length);
            current = MutationType.Randomize;
            switch (current)
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

                    arr[index] = (byte)_rand.Next(0, byte.MaxValue + 1);
                    break;
                case MutationType.Increment:
                    arr[index]++;
                    break;
                case MutationType.Decrement:
                    arr[index]--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private byte[] MixBytes(byte[] fBytes, byte[] mBytes)
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