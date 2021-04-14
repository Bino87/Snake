using System;
using System.Collections.Generic;
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
        public double[][] Calculate(Direction headDirection, Direction tailDirection, int mapSize, int foodX, int foodY, List<VisionData> visionData)
        {
            double[] input = GetInputValues(headDirection,tailDirection, mapSize, foodX, foodY, visionData);

            _result = _neuralNetwork.Evaluate(input);

            return _result;
        }

        (int HeadX, int HeadY) GetHead()
        {
            for(int x = 0; x < _map.GetLength(0); x++)
            {
                for(int y = 0; y < _map.GetLength(1); y++)
                {
                    if (_map[x, y].CellStatus == MapCellStatus.Head)
                        return (x, y);
                }
            }

            return (-1,-1);
        }

        private double[] GetInputValues(Direction headDirection, Direction tailDirection, int mapSize, int foodX, int foodY, List<VisionData> visionData)
        {
            double[] res = new double[_inputCount];
            int index = 0;

            (int headX, int headY) = GetHead();

            if (headX == -1 && headY == -1)
                return res;

            void GetValueAndSetInput(int x, int y)
            {
                (double value, double seesSelf, double seesFood) = GetValue(x, y, mapSize, mapSize,headX, headY, visionData);

                res[index++] = 1d - value;
                res[index++] = seesSelf;
                res[index++] = seesFood;
            }

            GetValueAndSetInput(-1, 0);
            GetValueAndSetInput(1, 0);
            GetValueAndSetInput(0, -1);
            GetValueAndSetInput(0, 1);
            GetValueAndSetInput(1, 1);
            GetValueAndSetInput(-1, 1);
            GetValueAndSetInput(1, -1);
            GetValueAndSetInput(-1, -1);

            //Head Direction
            res[index++] = headDirection == Direction.Up ? 1 : 0;
            res[index++] = headDirection == Direction.Left ? 1 : 0;
            res[index++] = headDirection == Direction.Down ? 1 : 0;
            res[index++] = headDirection == Direction.Right ? 1 : 0;

            //Tail Direction
            res[index++] = tailDirection == Direction.Up ? 1 : 0;
            res[index++] = tailDirection == Direction.Left ? 1 : 0;
            res[index++] = tailDirection == Direction.Down ? 1 : 0;
            res[index++] = tailDirection == Direction.Right ? 1 : 0;

            res[index++] = foodX > headX ? 1 : 0;
            res[index++] = foodX == headX ? 1 : 0;
            res[index++] = foodX < headX ? 1 : 0;
            res[index++] = foodY > headY ? 1 : 0;
            res[index++] = foodY == headY ? 1 : 0;
            res[index++] = foodY < headY ? 1 : 0;

            return res;
        }

        private (double value, double seesSelf, double seesFood) GetValue(int incX, int incY, double divideBy, int mapSize, int headX, int headY, List<VisionData> visionData)
        {
            double seesFood = 0;
            double seesSelf = 0;

            
            double value = 0;

            int x = headX;
            int y = headY;
           

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
                    case MapCellStatus.Head:
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
