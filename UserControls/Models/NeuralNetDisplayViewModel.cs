using System.Collections.ObjectModel;
using UserControls.Core.Base;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models
{
    public class NeuralNetDisplayViewModel : Observable
    {
        public ObservableCollection<PrimitiveShape> DisplayItems { get; set; }

        public NeuralNetDisplayViewModel()
        {
            DisplayItems = new ObservableCollection<PrimitiveShape>();
            DisplayItems.Add(new PrimitiveLine(100, 200, 100, 400));
            DisplayItems.Add(new PrimitiveLine(200, 100, 100, 400));
            DisplayItems.Add(new PrimitiveCircle(200, 300, 150,50));
        }
    }
}
