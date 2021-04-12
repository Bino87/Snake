using System;
using Network;
using Simulation.Core;
using Simulation.Enums;
using Simulation.Interfaces;

namespace Simulation
{
    internal class NetworkAgent
    {
        private readonly int _inputCount;
        private readonly IMapCell[,] _map;
        private readonly NeuralNetwork _neuralNetwork;
        private double[] _result;
        internal NetworkAgent(IMapCell[,] map, NetworkInfo networkInfo)
        {
            _map = map;
            _inputCount = networkInfo.InputCount;
            _result = new double[networkInfo.OutputCount];
            _neuralNetwork = new NeuralNetwork(networkInfo);
            
        }

        public NeuralNetwork GetNeuralNetwork() => _neuralNetwork;

        public double[] Calculate(SnakePart head, Direction tailDirection, int mapSize)
        {
            double[] input = GetInputValues(head, tailDirection, mapSize);

            _result = _neuralNetwork.Evaluate(input);

            return _result;
        }

        private double[] GetInputValues(SnakePart head, Direction tailDirection, int mapSize)
        {
            double[] res = new double[_inputCount];
            int index = 0;

            //Previous Results
            res[index] = _result[index++];
            res[index] = _result[index++];
            res[index] = _result[index++];
            res[index] = _result[index++];

            //Head Direction
            res[index++] = head.Direction == Direction.North ? 1 : 0;
            res[index++] = head.Direction == Direction.West ? 1 : 0;
            res[index++] = head.Direction == Direction.South ? 1 : 0;
            res[index++] = head.Direction == Direction.East ? 1 : 0;

            //Tail Direction
            res[index++] = tailDirection == Direction.North ? 1 : 0;
            res[index++] = tailDirection == Direction.West ? 1 : 0;
            res[index++] = tailDirection == Direction.South ? 1 : 0;
            res[index++] = tailDirection == Direction.East ? 1 : 0;


            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    (double value, bool seesSelf, bool seesFood) = GetValue(x, y, mapSize, head.X, head.Y, mapSize);

                    res[index++] = value;
                    res[index++] = seesSelf ? 1 : 0;
                    res[index++] = seesFood ? 1 : 0;
                }
            }

            return res;
        }

        private (double value, bool seesSelf, bool seesFood) GetValue(int incX, int incY, double divideBy, int headX, int headY, int mapSize)
        {
            bool seesFood = false;
            bool seesSelf = false;

            int x = headX + incX;
            int y = headY + incY;
            double value = 0;

            while (x >= 0 && x < mapSize && y >= 0 && y < mapSize)
            {
                IMapCell cell = _map[x, y];

                x += incX;
                y += incY;

                switch (cell.CellStatus)
                {
                    case MapCellStatus.Empty: value++; continue;
                    case MapCellStatus.Food: seesFood = true; break;
                    case MapCellStatus.Snake: seesSelf = true; break;
                    default: throw new ArgumentOutOfRangeException();
                }

                break;
            }

            return (value / divideBy, seesSelf, seesFood);
        }
    }
}
