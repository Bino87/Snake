using System;
using System.Data;
using Commons;

namespace Network.Mutators
{
    public class BitMutator : IMutator
    {
        private readonly double _mutationChancePercentage;
        private readonly double _mutationPercentage;
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
        }


        private bool debug = false;
        public (NetworkInfo First, NetworkInfo Second) GetOffsprings(NeuralNetwork father, NeuralNetwork mother)
        {
            NetworkInfo fNetworkInfo = father.ToNetworkInfo();
            NetworkInfo mNetworkInfo = mother.ToNetworkInfo();

            byte[] fBytes = fNetworkInfo.ToByteArr();
            byte[] mBytes = mNetworkInfo.ToByteArr();

            (byte[] first, byte[] second) = MixBytes(fBytes, mBytes);

            first = Mutate(first);
            second = Mutate(second);

            NetworkInfo firstNetworkInfo = fNetworkInfo.Copy();
            NetworkInfo secondNetworkInfo = fNetworkInfo.Copy();

            firstNetworkInfo.FromByteArray(first);
            secondNetworkInfo.FromByteArray(second);

            if (debug)
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("father");
                dt.Columns.Add("mother");
                dt.Columns.Add("first");
                dt.Columns.Add("second");

                for (int i = 0; i < fBytes.Length; i++)
                {

                    if (Check(fBytes[i], mBytes[i], first[i], second[i]))
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

        bool Check(params byte[] arr)
        {
            for (int x = 0; x < arr.Length; x++)
            {
                for (int z = 0; z < arr.Length; z++)
                {
                    if (x == z)
                        continue;
                    if (arr[x] != arr[z])
                        return true;
                }
            }

            return false;
        }

        private byte[] Mutate(byte[] arr)
        {
            if (RNG.Instance.NextDouble() < _mutationChancePercentage)
            {
                int mutationAmount = RNG.Instance.Next(1, (int) (_mutationPercentage * arr.Length + 1));

                for (int i = 0; i < mutationAmount; i++)
                {
                    int index = RNG.Instance.Next(arr.Length);
                    Mutate(arr, index);
                }
            }

            return arr;
        }

        private void Mutate(byte[] arr, int index)
        {
            MutationType[] enums = Enum.GetValues<MutationType>();
            //enums = new[] { MutationType.Decrement, MutationType.Increment, /*MutationType.SwapWithNext,MutationType.SwapWithPrevious*/};

            MutationType current = (MutationType)RNG.Instance.Next(enums.Length);
            //current = MutationType.Randomize;
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

                    arr[index] = (byte)RNG.Instance.Next(0, byte.MaxValue + 1);
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

        private (byte[] first, byte[] sec) MixBytes(byte[] fBytes, byte[] mBytes)
        {

            byte[] bytes1 = new byte[fBytes.Length];
            byte[] bytes2 = new byte[fBytes.Length];

            (int fLen, int mLen) = (fBytes.Length / 2, mBytes.Length / 2);

            int len = fBytes.Length;

            for (int i = 0; i < len; i++)
            {
                int a = RNG.Instance.Next(0, fLen + mLen);

                if (a < fLen)
                {
                    bytes1[i] = mBytes[i];
                    bytes2[i] = fBytes[i];
                    fLen--;
                }
                else
                {
                    bytes1[i] = fBytes[i];
                    bytes2[i] = mBytes[i];
                    mLen--;
                }
            }

            return (bytes1, bytes2);
        }
    }
}