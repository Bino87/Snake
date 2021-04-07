using System;
using System.Collections.Generic;
using Network;
using Simulation.Core;
using Simulation.Enums;

namespace Simulation
{
    internal class NetworkAgent
    {
        private double changeDirTreshold;
        private NeuralNetwork _neuralNetwork;
        internal NetworkAgent(double changeDirectionTreshhold, params int[] layersInts)     
        {
            changeDirTreshold = changeDirectionTreshhold;
            _neuralNetwork = new NeuralNetwork(new ReLu(),layersInts);
        }

        public Direction Calculate(HashSet<int> occupiedTiles, SnakePart head, Food food, Direction tailDirection, int mapSize)
        {
            double[] input = new double[0];

            //create input

            double[] results = _neuralNetwork.Evaluate(input);

            double max = Double.MinValue;
            int index = -1;

            for(int i = 0; i < results.Length; i++)
            {
                if (results[i] > max)
                {
                    max = results[i];
                    index = i;
                }
            }

            if(max < changeDirTreshold)
                return head.Direction;
            return (Direction)index;
        }
    }
}
