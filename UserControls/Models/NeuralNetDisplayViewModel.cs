using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Network;
using UserControls.Constants;
using UserControls.Core.Base;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models
{
    public class NeuralNetDisplayViewModel : Observable
    {
        public ObservableCollection<PrimitiveShape> DisplayItems { get; set; }
        private readonly PrimitiveShapeValueProvider[][] _lineValueProviders;
        private readonly PrimitiveShapeValueProvider[][] _circleValueProviders;
        const double radius = 10;
        public NeuralNetDisplayViewModel(NetworkInfo networkInfo)
        {
            DisplayItems = new ObservableCollection<PrimitiveShape>();
            _lineValueProviders = new PrimitiveShapeValueProvider[networkInfo.Layers + 1][];
            _circleValueProviders = new PrimitiveShapeValueProvider[networkInfo.Layers + 1][];



            Random rand = new();
            int offset = (int)((Cons.cNetWidth - 10) / 4);
            double w = Cons.cNetHeight / 32 - 1;
            double w2 = Cons.cNetHeight / 20 - 1;
            int start = 20;
            int end = start + 70;
            CreateNeurons(rand, start, w, 38, 15, 0);
            CreateNeurons(rand, start = start + offset, w, 20, 15, 1);
            CreateNeurons(rand, start = start + offset, w, 12, 15, 2);
            CreateNeurons(rand, start = start + offset, w2, 4, 15, 3);
            return;
            CreateConnections(rand, start, end, w, w2, 32, 20, 10, 15);

            CreateConnections(rand, start, end, w, w2, 20, 12, 15, 15);

            CreateConnections(rand, start, end, w, w2, 12, 4, 15, 15);


        }

        private void CreateConnections(Random rand, int start, int end, double w, double w2, int upper, int lower, double wOffset, double w2Offset)
        {
            for (int j = 0; j < upper; j++)
            {
                for (int i = 0; i < lower; i++)
                {
                    PrimitiveShapeValueProvider provider = new(rand.NextDouble(-10, 10));
                    //_lineValueProviders.Add(provider);
                    DisplayItems.Add(new PrimitiveLine(provider, start, end, j * w + wOffset, i * w2 + w2Offset));
                }
            }
        }

        private void CreateNeurons(Random rand, int x, double spaceing, int count, double wOffset, int layerIndex)
        {
            double offset = radius / 2;
            spaceing = (Cons.cNetHeight - 50) / count;
            _circleValueProviders[layerIndex] = new PrimitiveShapeValueProvider[count];

            for (int i = 0; i < count; i++)
            {
                PrimitiveShapeValueProvider provider = new(rand.NextDouble() * 2 - 1);
                _circleValueProviders[layerIndex][i] = provider;
                DisplayItems.Add(new PrimitiveCircle(provider, x - offset, i * spaceing - offset + wOffset, radius));
            }
        }

        public void OnResultsCalculated(double[][] results)
        {

            Application.Current.Dispatcher.Invoke(
                () =>
                    {

                        for (int i = 0; i < results.Length; i++)
                        {
                            for (int j = 0; j < results[i].Length; j++)
                            {
                                _circleValueProviders[i][j].Value = results[i][j];

                            }
                        }

                    }
                );
        }
    }
}
