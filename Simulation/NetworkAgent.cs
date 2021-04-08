using System;
using System.Collections.Generic;
using Network;
using Simulation.Core;
using Simulation.Enums;
using Math = System.Math;

namespace Simulation
{
    internal class NetworkAgent
    {
        private double changeDirTreshold;
        private int _inputCount;
        private NeuralNetwork _neuralNetwork;
        internal NetworkAgent(double changeDirectionTreshhold, params int[] layersInts)
        {
            changeDirTreshold = changeDirectionTreshhold;
            _inputCount = layersInts[0];
            _neuralNetwork = new NeuralNetwork(new ReLu(), layersInts);
        }

        public Direction Calculate(HashSet<int> occupiedTiles, SnakePart head, Food food, Direction tailDirection, int mapSize)
        {
            double[] input = GetInputValues(occupiedTiles, head, food, tailDirection, mapSize);

            double[] results = _neuralNetwork.Evaluate(input);

            double max = Double.MinValue;
            int index = -1;

            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] > max)
                {
                    max = results[i];
                    index = i;
                }
            }

            //if (max > changeDirTreshold)
            //    return head.Direction;
            return (Direction)index;
        }

        private double[] GetInputValues(HashSet<int> occTiles, SnakePart head, Food food, Direction tailDirection, int mapSize)
        {
            double[] res = new double[_inputCount];
            int index = 0;
            int fIndex = food.InternalIndex;

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

            int hIndex = head.InternalIndex;

            bool seesSelf, seesFood;
            //north
            int steps = hIndex / mapSize;

            double GetValue(int increment)
            {
                seesFood = false;
                seesSelf = false;
                int current = hIndex + increment;

                for (int i = 0; i < steps; i++)
                {
                    if (occTiles.Contains(index))
                    {
                        seesSelf = true;
                        return i;
                    }
                    if (fIndex == current)
                    {
                        seesFood = true;
                        return i;
                    }

                    current += increment;

                    if (current < 0 || current > mapSize * mapSize)
                        return i;
                }


                return steps; ;
            }

            res[index++] = GetValue(-mapSize) / mapSize;
            res[index++] = seesSelf ? 1 : 0;
            res[index++] = seesFood ? 1 : 0;

            //northWest
            steps = Math.Min(hIndex / mapSize, mapSize - hIndex % mapSize);
            res[index++] = GetValue(-mapSize + 1) / mapSize * 1.44;
            res[index++] = seesSelf ? 1 : 0;
            res[index++] = seesFood ? 1 : 0;

            //west 
            steps = mapSize - hIndex % mapSize;
            res[index++] = GetValue(1) / mapSize;
            res[index++] = seesSelf ? 1 : 0;
            res[index++] = seesFood ? 1 : 0;

            //southWest
            steps = Math.Min(mapSize - hIndex % mapSize, mapSize - hIndex / mapSize);
            res[index++] = GetValue(mapSize + 1) / mapSize;
            res[index++] = seesSelf ? 1 : 0;
            res[index++] = seesFood ? 1 : 0;

            //south
            steps = mapSize - hIndex / mapSize;
            res[index++] = GetValue(mapSize) / mapSize;
            res[index++] = seesSelf ? 1 : 0;
            res[index++] = seesFood ? 1 : 0;

            //southEast
            steps = Math.Min(hIndex % mapSize, mapSize - hIndex / mapSize);
            res[index++] = GetValue(mapSize + 1) / mapSize * 1.44;
            res[index++] = seesSelf ? 1 : 0;
            res[index++] = seesFood ? 1 : 0;

            //east
            steps = hIndex % mapSize;
            res[index++] = GetValue(-1) / mapSize;
            res[index++] = seesSelf ? 1 : 0;
            res[index++] = seesFood ? 1 : 0;

            //northEast
            steps = Math.Min(hIndex % mapSize, hIndex / mapSize);
            res[index++] = GetValue(-mapSize - 1) / mapSize * 1.44;
            res[index++] = seesSelf ? 1 : 0;
            res[index++] = seesFood ? 1 : 0;

            return res;
        }

        
    }
}
