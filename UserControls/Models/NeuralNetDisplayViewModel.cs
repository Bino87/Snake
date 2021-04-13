using System;
using System.Collections.ObjectModel;
using Network;
using Simulation;
using UserControls.Constants;
using UserControls.Core.Base;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models
{
    public class NeuralNetDisplayViewModel : Observable
    {
        public ObservableCollection<PrimitiveShape> DisplayItems { get; set; }

        public NeuralNetDisplayViewModel(NetworkInfo networkInfo)
        {
            DisplayItems = new ObservableCollection<PrimitiveShape>();


            double radius = 7.5;

            Random rand = new();

            double w = Cons.cNetHeight / 32 - 1;
            double w2 = Cons.cNetHeight / 20 - 1;
            int start = 20;
            int end = start + 70;

            CreateConnections(rand, start, end, w, w2, 32,20);
            CreateNeurons(rand, start, w, 32, radius);

            start = end;
            end = start + 70;
            w = w2;
            w2 = Cons.cNetHeight / 12 - 1;
           
            CreateConnections(rand, start, end, w, w2, 20, 12);
            CreateNeurons(rand, start, w, 20, radius);

            start = end;
            end = start + 70;
            w = w2;
            w2 = Cons.cNetHeight / 4 - 1;
            CreateConnections(rand, start, end, w, w2, 12,4);

          

            CreateNeurons(rand, start, w, 12, radius);
            CreateNeurons(rand, end, w2, 4, radius);



        }

        private void CreateConnections(Random rand, int start, int end, double w, double w2, int upper, int lower)
        {

            for(int j = 0; j < upper; j++)
            {
                for(int i = 0; i < lower; i++)
                {
                    DisplayItems.Add(new PrimitiveLine(new PrimitiveShapeValueProvider(rand.NextDouble() * 2 - 1), start, end, j * w, i * w2));
                }
            }
        }

        private void CreateNeurons(Random rand, int x, double w, int count, double radius)
        {
            double offset = radius / 2;

            for(int i = 0; i < count; i++)
            {
                DisplayItems.Add(new PrimitiveCircle(new PrimitiveShapeValueProvider(rand.NextDouble() * 2 - 1), x - offset, i * w - offset, radius));
            }
        }
    }
}
