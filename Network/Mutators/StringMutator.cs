using System.Text;
using Commons;
using Commons.Extensions;

namespace Network.Mutators
{

    public class StringMutator : IMutator
    {
        private readonly double _mutationChancePercentage;
        private readonly double _mutationPercentage;
        private readonly int _minSubstringLength;
        private readonly int _maxSubstringLength;


        public StringMutator(double mutationChancePercentage, double mutationPercentage, int minSubstringLen, int maxSubstringLen)
        {
            _mutationChancePercentage = mutationChancePercentage;
            _mutationPercentage = mutationPercentage;
            _minSubstringLength = minSubstringLen;
            _maxSubstringLength = maxSubstringLen;
        }

        public (NetworkData First, NetworkData Second) Get2Offsprings(BasicNeuralNetwork parent1, BasicNeuralNetwork parent2)
        {
            NetworkData fNetworkData = parent1.CopyNetworkInfo();
            NetworkData mNetworkData = parent2.CopyNetworkInfo();

            (string fBinaryString, int fArrLen) = ConvertToBinaryString(fNetworkData);
            (string mBinaryString, int mArrLen) = ConvertToBinaryString(mNetworkData);

            byte[] firstBytes = CreateArray(Mutate(Mix(fBinaryString, mBinaryString)), fArrLen);
            byte[] secondBytes = CreateArray(Mutate(Mix(mBinaryString, fBinaryString)), mArrLen);

            fNetworkData.FromByteArray(firstBytes);
            mNetworkData.FromByteArray(secondBytes);

            return (fNetworkData, mNetworkData);
        }

        private static (string Value, int byteArrLenght) ConvertToBinaryString(NetworkData networkData)
        {
            byte[] arr = networkData.ToByteArr();

            return (arr.ToBinaryString(), arr.Length);
        }

        private static byte[] CreateArray(string str, int arrLen)
        {
            byte[] arr = new byte[arrLen];

            for (int i = 0; i < arrLen; i++)
            {
                string subString = str.Substring(i * 8, 8);
                arr[i] = subString.ToByte(2);
            }

            return arr;
        }

        private string Mutate(string str)
        {
            if (RNG.Instance.NextDouble01() >= _mutationChancePercentage)
                return str;
            int amount = RNG.Instance.Next(1, (int)(str.Length * _mutationPercentage));

            StringBuilder sb = new(str);

            for (int i = 0; i < amount; i++)
            {
                int index = RNG.Instance.Next(0, str.Length);

                sb[index] = sb[index] == '1' ? '0' : '1';
            }

            return sb.ToString();
        }

        private string Mix(string fStr, string mStr)
        {
            StringBuilder sb = new();

            int fCount = fStr.Length / 2;
            int mCount = mStr.Length / 2;

            int geneLen;
            for (int i = 0; i < fStr.Length; i += geneLen)
            {
                geneLen = RNG.Instance.Next(_minSubstringLength, _maxSubstringLength);

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