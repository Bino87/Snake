using System;
using System.Text;
using Commons;
using Commons.Extensions;

namespace Network.Mutators
{

    public class StringMutator : IMutator
    {
        private readonly double _mutationChancePercentage;
        private readonly double _mutationPercentage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mutationChancePercentage"> Set value from 0.0 to 1.0</param>
        /// <param name="mutationPercentage">Set value from 0.0 to 1.0</param>
        public StringMutator(double mutationChancePercentage, double mutationPercentage)
        {
            _mutationChancePercentage = mutationChancePercentage;
            _mutationPercentage = mutationPercentage;
        }

        public (NetworkInfo First, NetworkInfo Second) GetOffsprings(NeuralNetwork parent1, NeuralNetwork parent2)
        {
            NetworkInfo fNetworkInfo = parent1.CopyNetworkInfo();
            NetworkInfo mNetworkInfo = parent2.CopyNetworkInfo();

            byte[] fBytes = fNetworkInfo.ToByteArr();
            byte[] mBytes = mNetworkInfo.ToByteArr();

            string fStr = fBytes.ToBinaryString();
            string mStr = mBytes.ToBinaryString();

            byte[] firstBytes = CreateArray(Mutate(Mix(fStr, mStr)), fBytes.Length);
            byte[] secondBytes = CreateArray(Mutate(Mix(mStr, fStr)), mBytes.Length);

            fNetworkInfo.FromByteArray(firstBytes);
            mNetworkInfo.FromByteArray(secondBytes);

            return (fNetworkInfo, mNetworkInfo);

        }

        static byte[] CreateArray(string str, int arrLen)
        {
            byte[] arr = new byte[arrLen];

            for (int i = 0; i < arrLen; i++)
            {
                string subString = str.Substring(i * 8, 8);
                arr[i] = Convert.ToByte(subString, 2);
            }

            return arr;
        }

        private string Mutate(string str)
        {
            if (RNG.Instance.NextDouble01() < _mutationChancePercentage)
            {
                int amount = RNG.Instance.Next(1, (int) (str.Length * _mutationPercentage));
                
                StringBuilder sb = new(str);

                for(int i = 0; i < amount; i++)
                {
                    int index = RNG.Instance.Next(0, str.Length);

                    sb[index] = sb[index] == '1' ? '0' : '1';
                }

                return sb.ToString();
            }

            return str;

        }

        string Mix(string fStr, string mStr)
        {
            StringBuilder sb = new();

            int fCount = fStr.Length / 2;
            int mCount = mStr.Length / 2;

            int geneLen;
            for (int i = 0; i < fStr.Length; i += geneLen)
            {
                geneLen = RNG.Instance.Next(2, 12);

                string substring;
                if (RNG.Instance.Next(fCount + mCount) < fCount)
                {
                    if (geneLen > fCount) 
                        geneLen = fCount;

                    fCount -= geneLen;
                    substring = fStr.Substring(i, geneLen);
                }
                else
                {
                    if (geneLen > mCount) 
                        geneLen = mCount;
                    mCount -= geneLen;
                    substring = mStr.Substring(i, geneLen);
                }

                sb.Append(substring);
            }

            return sb.ToString();
        }
    }
}