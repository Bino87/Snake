using System;
using System.Collections.Generic;
using Network;
using Simulation.Core;
using Simulation.Enums;

namespace Simulation
{
    internal class NetworkAgent
    {
        private readonly int _inputCount;
        private readonly NeuralNetwork _neuralNetwork;
        private double[][] _result;
        internal NetworkAgent(NetworkInfo networkInfo)
        {
            _inputCount = networkInfo.InputCount;
            _neuralNetwork = new NeuralNetwork(networkInfo);
        }

        public NeuralNetwork GetNeuralNetwork() => _neuralNetwork;
        public double[][] Calculate(NetworkAgentCalculationParameters parameters, List<VisionData> visionData)
        {
            double[] input = GetInputValues(parameters, visionData);

            _result = _neuralNetwork.Evaluate(input);

            return _result;
        }

        //private double[] GetInputValues(Direction headDirection, Direction tailDirection, int mapSize, int foodX, int foodY, ICollection<VisionData> visionData, Dictionary<(int, int), MapCellType> mapCellTypes)
        private double[] GetInputValues(NetworkAgentCalculationParameters parameters, List<VisionData> visionData)
        {
            double[] res = new double[_inputCount];
            int index = 0;


            void GetValueAndSetInput(int x, int y)
            {
                (double value, bool seesSelf, bool seesFood) = GetValue(x, y, parameters, visionData);

                res[index++] = 1d - value;
                res[index++] = seesSelf ? 1 : 0;
                res[index++] = seesFood ? 1 : 0;
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
            res[index++] = parameters.Head.Direction == Direction.Up ? 1 : 0;
            res[index++] = parameters.Head.Direction == Direction.Left ? 1 : 0;
            res[index++] = parameters.Head.Direction == Direction.Down ? 1 : 0;
            res[index++] = parameters.Head.Direction == Direction.Right ? 1 : 0;

            //Tail Direction
            res[index++] = parameters.Tail.Direction == Direction.Up ? 1 : 0;
            res[index++] = parameters.Tail.Direction == Direction.Left ? 1 : 0;
            res[index++] = parameters.Tail.Direction == Direction.Down ? 1 : 0;
            res[index++] = parameters.Tail.Direction == Direction.Right ? 1 : 0;

            //res[index++] = parameters.Food.X > parameters.Head.X ? 1 : 0;
            //res[index++] = parameters.Food.X == parameters.Head.X ? 1 : 0;
            //res[index++] = parameters.Food.X < parameters.Head.X ? 1 : 0;
            //res[index++] = parameters.Food.Y > parameters.Head.Y ? 1 : 0;
            //res[index++] = parameters.Food.Y == parameters.Head.Y ? 1 : 0;
            //res[index++] = parameters.Food.Y < parameters.Head.Y ? 1 : 0;

            return res;
        }

        //private (double value, double seesSelf, double seesFood) GetValue(int incX, int incY, double divideBy, int mapSize, int headX, int headY, ICollection<VisionData> visionData, Dictionary<(int, int), MapCellType> mapCellTypes)
        private (double value, bool seesSelf, bool seesFood) GetValue(int incX, int incY, NetworkAgentCalculationParameters parameters, List<VisionData> visionData)
        {
            bool seesFood = false;
            bool seesSelf = false;


            double value = 0;

            int x = parameters.Head.X;
            int y = parameters.Head.Y;

            int headX = parameters.Head.X;
            int headY = parameters.Head.Y;

            void Increment()
            {
                x += incX;
                y += incY;
            }

            while (x >= 0 && x < parameters.MapSize && y >= 0 && y < parameters.MapSize)
            {
                if (parameters.LookUp.TryGetValue((x, y), out MapCellType item))
                {
                    switch (item)
                    {
                        case MapCellType.Food:
                            {
                                seesFood = true;
                                visionData.Add(new VisionData(VisionCollisionType.Food, headX, x, headY, y));
                            }
                            break;
                        case MapCellType.Snake:
                            if (x == headX && y == headY)
                                break;
                            {
                                seesSelf = true;
                                visionData.Add(new VisionData(VisionCollisionType.Self, headX, x, headY, y));
                            }
                            break;
                        case MapCellType.Head:
                            break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }

                Increment();
                value++;
            }

            visionData.Add(new VisionData(VisionCollisionType.Normal, headX, x, headY, y));

            return (value / parameters.MapSize, seesSelf, seesFood);
        }
    }
}
