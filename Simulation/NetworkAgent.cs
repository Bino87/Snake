using System;
using System.Collections.Generic;
using System.Text;
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
        private double[][] _result;
        internal NetworkAgent(IMapCell[,] map, NetworkInfo networkInfo)
        {
            _map = map;
            _inputCount = networkInfo.InputCount;
            _neuralNetwork = new NeuralNetwork(networkInfo);
        }

        public NeuralNetwork GetNeuralNetwork() => _neuralNetwork;
        public double[][] Calculate(SnakePart head, Direction tailDirection, int mapSize, int foodX, int foodY, List<VisionData> visionData)
        {
            double[] input = GetInputValues(head, tailDirection, mapSize, foodX, foodY, visionData);

            _result = _neuralNetwork.Evaluate(input);

            return _result;
        }

        private double[] GetInputValues(SnakePart head, Direction tailDirection, int mapSize, int foodX, int foodY, List<VisionData> visionData)
        {
            double[] res = new double[_inputCount];
            int index = 0;


            void DUDD(int x, int y)
            {
                (double value, double seesSelf, double seesFood) = GetValue(x, y, mapSize, head.X, head.Y, mapSize, visionData);

                res[index++] = 1d - value;
                res[index++] = seesSelf;
                res[index++] = seesFood;
            }

            DUDD(-1, 0);
            DUDD(1, 0);
            DUDD(0, -1);
            DUDD(0, 1);
            DUDD(1, 1);
            DUDD(-1, 1);
            DUDD(1, -1);
            DUDD(-1, -1);

            //Head Direction
            res[index++] = head.Direction == Direction.Up ? 1 : 0;
            res[index++] = head.Direction == Direction.Left ? 1 : 0;
            res[index++] = head.Direction == Direction.Down ? 1 : 0;
            res[index++] = head.Direction == Direction.Right ? 1 : 0;

            //Tail Direction
            res[index++] = tailDirection == Direction.Up ? 1 : 0;
            res[index++] = tailDirection == Direction.Left ? 1 : 0;
            res[index++] = tailDirection == Direction.Down ? 1 : 0;
            res[index++] = tailDirection == Direction.Right ? 1 : 0;

            res[index++] = foodX > head.X ? 1 : 0;
            res[index++] = foodX == head.X ? 1 : 0;
            res[index++] = foodX < head.X ? 1 : 0;
            res[index++] = foodY > head.Y ? 1 : 0;
            res[index++] = foodY == head.Y ? 1 : 0;
            res[index++] = foodY < head.Y ? 1 : 0;

            return res;
        }

        private (double value, double seesSelf, double seesFood) GetValue(int incX, int incY, double divideBy, int headX, int headY, int mapSize, List<VisionData> visionData)
        {
            double seesFood = 0;
            double seesSelf = 0;

            int x = headX;
            int y = headY;
            double value = 0;

            void Increment()
            {
                x += incX;
                y += incY;
            }

            while (x >= 0 && x < mapSize && y >= 0 && y < mapSize)
            {
                IMapCell cell = _map[x, y];

                switch (cell.CellStatus)
                {
                    case MapCellStatus.Food:
                        if (seesFood == 0)
                        {
                            seesFood = 1;
                            visionData.Add(new VisionData(VisionCollisionType.Food, headX, x, headY, y));
                        }
                        break;
                    case MapCellStatus.Snake:
                        if (x == headX && y == headY)
                            break;
                        if (seesSelf == 0)
                        {
                            seesSelf = 1;
                            visionData.Add(new VisionData(VisionCollisionType.Self, headX, x, headY, y));
                        }
                        break;
                    case MapCellStatus.Empty:
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }

                Increment();
                value++;
            }

            visionData.Add(new VisionData(VisionCollisionType.Normal, headX, x, headY, y));

            return (value / divideBy, seesSelf, seesFood);
        }
    }
}
