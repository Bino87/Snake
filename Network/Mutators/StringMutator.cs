using System;
using System.Data;
using System.Text;

namespace Network.Mutators
{

    public class StringMutator : IMutator
    {
        private readonly double _mutationChancePercentage;
        private readonly double _mutationPercentage;
        private readonly Random _rand;
        private readonly int _minCopyLen;
        private readonly int _maxCopyLen;
        private bool debug;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mutationChancePercentage"> Set value from 0.0 to 1.0</param>
        /// <param name="mutationPercentage">Set value from 0.0 to 1.0</param>
        public StringMutator(double mutationChancePercentage, double mutationPercentage, int minCopyLen, int maxCopyLen)
        {
            _mutationChancePercentage = mutationChancePercentage;
            _mutationPercentage = mutationPercentage;
            _minCopyLen = minCopyLen;
            _maxCopyLen = maxCopyLen;
            _rand = new Random();
        }

        public (NetworkInfo First, NetworkInfo Second) GetOffsprings(NeuralNetwork father, NeuralNetwork mother)
        {
            NetworkInfo fNetworkInfo = father.ToNetworkInfo();
            NetworkInfo mNetworkInfo = mother.ToNetworkInfo();

            byte[] fBytes = fNetworkInfo.ToByteArr();
            byte[] mBytes = mNetworkInfo.ToByteArr();

            string fStr = ToBinaryString(fBytes);
            string mStr = ToBinaryString(mBytes);

            byte[] firstBytes = CreateArray(Mutate(Mix(fStr, mStr)), fBytes.Length);
            byte[] secondBytes = CreateArray(Mutate(Mix(mStr, fStr)), fBytes.Length);

            fNetworkInfo.FromByteArray(firstBytes);
            fNetworkInfo.FromByteArray(secondBytes);

            if (debug)
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
                        firstBytes[i],
                        secondBytes[i]
                    );
                }
            }



            return (fNetworkInfo, mNetworkInfo);

        }

        static byte[] CreateArray(string str, int arrLen)
        {
            byte[] arr = new byte[arrLen];

            for (int i = 0; i < arrLen; i++)
            {
                var subString = str.Substring(i * 8, 8);
                arr[i] = Convert.ToByte(subString, 2);
            }

            return arr;
        }

        private string Mutate(string str)
        {
            if (_rand.NextDouble() < _mutationChancePercentage)
            {
                int amount = (int)(str.Length * _mutationPercentage);

                StringBuilder sb = new(str);

                for (int i = 0; i < amount; i++)
                {
                    int index = _rand.Next(str.Length);
                    if (sb[index] == '1')
                        sb[index] = '0';
                    else sb[index] = '1';
                }

                return sb.ToString();
            }

            return str;

        }

        string Mix(string fStr, string mStr)
        {
            StringBuilder sb = new();

            int fCount = fStr.Length;
            int mCount = mStr.Length;


            for (int i = 0; i < fStr.Length;)
            {
                int amount = _rand.Next(_minCopyLen, _maxCopyLen);

                if (i + amount >= fStr.Length)
                    amount = fStr.Length - i;

                int random = _rand.Next(2);

                string substring;
                if (random == 0)
                {
                    fCount -= amount;
                    substring = fStr.Substring(i, amount);
                }
                else
                {
                    mCount -= amount;
                    substring = mStr.Substring(i, amount);
                }

                sb.Append(substring);
                i += amount;
            }

            return sb.ToString();
        }

        private static string ToBinaryString(byte[] arr)
        {
            StringBuilder sb = new();

            for (int i = 0; i < arr.Length; i++)
            {
                string str = Convert.ToString(arr[i], 2).PadLeft(8, '0');

                sb.Append(str);
            }

            return sb.ToString();
        }
    }
}