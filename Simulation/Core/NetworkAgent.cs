using System;
using Network;
using Simulation.Enums;
using Simulation.Extensions;
using Simulation.Interfaces;

namespace Simulation.Core
{
    internal class NetworkAgent
    {
        private readonly int _inputCount;
        private readonly BasicNeuralNetwork _basicNeuralNetwork;
        private double[][] _result;
        internal NetworkAgent(NetworkInfo networkInfo)
        {
            _inputCount = networkInfo.InputCount;
            _basicNeuralNetwork = new BasicNeuralNetwork(networkInfo);
            _result = new[] { new double[3] };
        }

        public BasicNeuralNetwork GetNeuralNetwork() => _basicNeuralNetwork;
        public double[][] Calculate(NetworkAgentCalculationParameters parameters, IUpdate<IOnMoveUpdateParameters> updater)
        {
            double[] input = GetInputValues(parameters, updater);

            _result = _basicNeuralNetwork.Evaluate(input);

            return _result;
        }

        private double[] GetInputValues(NetworkAgentCalculationParameters parameters, IUpdate<IOnMoveUpdateParameters> updater)
        {
            double[] res = new double[_inputCount];
            int index = 0;


            void GetValueAndSetInput(int x, int y)
            {
                (double value, double seesSelf, double seesFood) = GetValue(x, y, parameters, updater);

                int X = parameters.Head.X + x;
                int Y = parameters.Head.Y + y;

                bool aa = X < 0 || Y < 0 || X >= parameters.MapSize || Y >= parameters.MapSize;
                bool bb = X == 1 || Y == 1 || X == parameters.MapSize - 1 || Y == parameters.MapSize - 1;
                res[index++] = aa ? 1 : 0;
                res[index++] = bb ? 1 : 0;
                res[index++] = value;
                res[index++] = seesSelf > 1 ? 1 : 0;
                res[index++] = seesSelf;
                res[index++] = seesFood > 1 ? 1 : 0;
                res[index++] = seesFood;
            }

            GetValueAndSetInput(-1, -1);
            GetValueAndSetInput(-1, 0);
            GetValueAndSetInput(-1, 1);
            GetValueAndSetInput(0, -1);
            GetValueAndSetInput(0, 1);
            GetValueAndSetInput(1, -1);
            GetValueAndSetInput(1, 0);
            GetValueAndSetInput(1, 1);

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

            res[index++] = parameters.Food.X > parameters.Head.X ? 1 : 0;
            res[index++] = parameters.Food.X == parameters.Head.X ? 1 : 0;
            res[index++] = parameters.Food.X < parameters.Head.X ? 1 : 0;
            res[index++] = parameters.Food.Y > parameters.Head.Y ? 1 : 0;
            res[index++] = parameters.Food.Y == parameters.Head.Y ? 1 : 0;
            res[index++] = parameters.Food.Y < parameters.Head.Y ? 1 : 0;

            for (int i = 0; i < _result[^1].Length; i++)
            {
                res[index++] = _result[^1][i];
            }

            return res;
        }

        private static (double value, double seesSelf, double seesFood) GetValue(int incX, int incY, NetworkAgentCalculationParameters parameters, IUpdate<IOnMoveUpdateParameters> updater)
        {
            double? seesFood = null;
            double? seesSelf = null;
            double value = 0;

            int x = parameters.Head.X;
            int y = parameters.Head.Y;

            int headX = parameters.Head.X;
            int headY = parameters.Head.Y;

            static double Distance(int origin, int target) => Math.Sqrt(origin * origin + target * target);

            bool ShouldKeepRuning()
            {
                int tempX = x + incX;
                int tempY = y + incY;

                if (tempX < 0) return false;
                if (tempX >= parameters.MapSize) return false;
                if (tempY < 0) return false;
                if (tempY >= parameters.MapSize) return false;

                x = tempX;
                y = tempY;

                return true;
            }

            while (ShouldKeepRuning())
            {
                if (parameters.LookUp.TryGetValue((x, y), out MapCellType item))
                {
                    switch (item)
                    {
                        case MapCellType.Food:
                            {
                                double dist = Distance(headX - x, headY - y);

                                seesFood = seesFood.HasValue ? Math.Min(dist, seesFood.Value) : dist;
                                updater.TryAddVisionData(headX, headY, x, y, VisionCollisionType.Food);
                            }
                            break;
                        case MapCellType.Snake:
                            if (x == headX && y == headY)
                                break;
                            {
                                double dist = Distance(headX - x, headY - y);
                                seesSelf = seesSelf.HasValue ? Math.Min(dist, seesSelf.Value) : dist;
                                updater.TryAddVisionData(headX, headY, x, y, VisionCollisionType.Self);
                            }
                            break;
                        case MapCellType.Head:
                            break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }


                value++;
            }

            updater.TryAddVisionData(headX, headY, x, y, VisionCollisionType.Normal);

            return (value / Distance(0, parameters.MapSize),
                seesSelf.HasValue ? seesSelf.Value / parameters.MapSize : 0,
                seesFood.HasValue ? seesFood.Value / parameters.MapSize : 0);
        }
    }
}
