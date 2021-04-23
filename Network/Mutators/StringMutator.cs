﻿using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using Commons;

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
            if (RNG.Instance.NextDouble() < _mutationChancePercentage)
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

            int a = 4;

            int fCount = fStr.Length / (a / 2);
            int mCount = mStr.Length / (a / 2);


            for (int i = 0; i < fStr.Length; i += a)
            {
                int random = RNG.Instance.Next(fCount + mCount);

                string substring;
                if (random < fCount)
                {
                    fCount -= a;
                    substring = fStr.Substring(i, a);
                }
                else
                {
                    mCount -= a;
                    substring = mStr.Substring(i, a);
                }

                sb.Append(substring);
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