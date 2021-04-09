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
        private int _inputCount;
        private IMapCell[,] _map;
        private NeuralNetwork _neuralNetwork;
        private double[] result;
        internal NetworkAgent(IMapCell[,] map, params LayerInfo[] layerInfos)
        {
            _map = map;
            _inputCount = layerInfos[0].NodeCount;

            result = new double[layerInfos[^1].NodeCount];

            _neuralNetwork = new NeuralNetwork(new NetworkInfo(layerInfos));
        }

        public Direction Calculate(SnakePart head, Food food, Direction tailDirection, int mapSize)
        {
            double[] input = GetInputValues(head, tailDirection, mapSize);

            result = _neuralNetwork.Evaluate(input);

            double max = Double.MinValue;
            int index = -1;

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] > max)
                {
                    max = result[i];
                    index = i;
                }
            }

            return (Direction)index;
        }

        private double[] GetInputValues(SnakePart head, Direction tailDirection, int mapSize)
        {
            double[] res = new double[_inputCount];
            int index = 0;

            //Previous Results
            res[index] = result[index++];
            res[index] = result[index++];
            res[index] = result[index++];
            res[index] = result[index++];

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



            bool seesSelf, seesFood;

            double GetValue(int incX, int incY, double divideBy)
            {
                seesFood = false;
                seesSelf = false;

                int x = head.X + incX;
                int y = head.Y + incY;
                double value = 0;

                while (x >= 0 && x < mapSize && y >= 0 && y < mapSize)
                {
                    IMapCell cell = _map[x, y];

                    x += incX;
                    y += incY;

                    switch (cell.CellStatus)
                    {
                        case MapCellStatus.Empty:
                            value++;
                            continue;
                            break;
                        case MapCellStatus.Food:
                            seesFood = true;
                            break;
                        case MapCellStatus.Snake:
                            seesSelf = true;
                            break;
                        default: throw new ArgumentOutOfRangeException();
                    }

                    break;
                }

                return value / divideBy;
            }

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    res[index++] = GetValue(x, y, mapSize);
                    res[index++] = seesSelf ? 1 : 0;
                    res[index++] = seesFood ? 1 : 0;

                }
            }


            return res;
        }


    }
}
