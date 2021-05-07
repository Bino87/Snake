using UserControls.Core.Base;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models.NeuralNetWizard
{
    public class NeuralNetWizardViewModel : MainView
    {
        public NeuralNetDisplayViewModel NeuralNetDisplay { get; set; }

        public NeuralNetWizardViewModel()
        {
            NeuralNetDisplay = new NeuralNetDisplayViewModel();
        }

        public override void Abort()
        {
            
        }
    }
}