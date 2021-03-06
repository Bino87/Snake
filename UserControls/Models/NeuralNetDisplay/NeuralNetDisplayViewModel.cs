using System.Collections.Generic;
using System.Collections.ObjectModel;
using Network;
using UserControls.Constants;
using UserControls.Core.Base;
using UserControls.Core.Objects.NeuralNetDisplay;

namespace UserControls.Models.NeuralNetDisplay
{
    public class NeuralNetDisplayViewModel : Observable
    {
        private const double cAvailible = Cons.cNetHeight - (2 * Cons.cNetHeightPadding);

        private PrimitiveShapeValueProvider[][] _lineValueProviders;
        private PrimitiveShapeValueProvider[][] _circleValueProviders;
        private const double cRadius = 10;

        public NetworkTemplate NetworkTemplate { get; private set; }

        public ObservableCollection<PrimitiveShape> DisplayItems { get; set; }


        public NeuralNetDisplayViewModel()
        {
            DisplayItems = new ObservableCollection<PrimitiveShape>();
        }

        public NeuralNetDisplayViewModel(NetworkTemplate networkData) : this()
        {
            Initialize(networkData);
        }

        public void Initialize(NetworkTemplate networkTemplate)
        {
            Clear();
            NetworkTemplate = networkTemplate;
            _lineValueProviders = new PrimitiveShapeValueProvider[NetworkTemplate.Layers][];
            _circleValueProviders = new PrimitiveShapeValueProvider[NetworkTemplate.Layers + 1][];

            CreateConnections();
        }

        private void CreateConnections()
        {
            double offset = (Cons.cNetWidth - 2 * Cons.cNetWidthPadding) / NetworkTemplate.Layers;
            double start = Cons.cNetWidthPadding;

            for (int i = 1; i < NetworkTemplate.LayerSetup.Length; i++)
            {
                CreateConnections(start, start + offset, NetworkTemplate.LayerSetup[i - 1], NetworkTemplate.LayerSetup[i], i - 1);
                start += offset;
            }

            offset = (Cons.cNetWidth - 2 * Cons.cNetWidthPadding) / NetworkTemplate.Layers;
            start = Cons.cNetWidthPadding;


            for (int i = 0; i < NetworkTemplate.LayerSetup.Length; i++)
            {
                CreateNeuronLayer(start, NetworkTemplate.LayerSetup[i], i);
                start += offset;
            }
        }

        private void CreateConnections(double start, double end, int upper, int lower, int layerIndex)
        {

            double verticalSpacingA = upper == 1 ? cAvailible / 2 : cAvailible / (upper - 1);
            double verticalSpacingB = lower == 1 ? cAvailible / 2 : cAvailible / (lower - 1);
            _lineValueProviders[layerIndex] = new PrimitiveShapeValueProvider[lower * upper];

            for (int j = 0; j < upper; j++)
            {
                for (int i = 0; i < lower; i++)
                {
                    PrimitiveShapeValueProvider provider = new();
                    provider.Value = 0;
                    _lineValueProviders[layerIndex][j * lower + i] = provider;
                    DisplayItems.Add(new PrimitiveLine(provider, start, end, j * verticalSpacingA + Cons.cNetHeightPadding, i * verticalSpacingB + Cons.cNetHeightPadding));
                }
            }
        }

        private void CreateNeuronLayer(double x, int count, int layerIndex)
        {
            double offset = cRadius / 2;

            double verticalSpacing = count == 1 ? cAvailible / 2 : cAvailible / (count - 1);
            double wOffset = Cons.cNetHeightPadding;

            _circleValueProviders[layerIndex] = new PrimitiveShapeValueProvider[count];

            for (int i = 0; i < count; i++)
            {
                PrimitiveShapeValueProvider provider = new();

                _circleValueProviders[layerIndex][i] = provider;
                DisplayItems.Add(new PrimitiveCircle(provider, x - offset, i * verticalSpacing - offset + wOffset, cRadius));
            }
        }

        public void UpdateWeights(IList<double[]> weights)
        {
            for (int i = 0; i < weights.Count; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    double d = weights[i][j];

                    _lineValueProviders[i][j].Value = d;
                }
            }
        }

        public void OnResultsCalculated(IList<double[]> results)
        {
            for (int i = 0; i < results.Count; i++)
            {
                for (int j = 0; j < results[i].Length; j++)
                {
                    _circleValueProviders[i][j].Value = results[i][j];
                }
            }
        }

        public void Clear()
        {
            NetworkTemplate = null;
            DisplayItems.Clear();
        }
    }
}
