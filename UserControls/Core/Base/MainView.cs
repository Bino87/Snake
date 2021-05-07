using System;
using UserControls.Core.Enums;
using UserControls.Models.NeuralNetWizard;
using UserControls.Models.SimulationRunner;

namespace UserControls.Core.Base
{
    public abstract class MainView : Observable
    {
        internal static MainView GetView(ViewModel viewModel)
            => viewModel switch
            {
                ViewModel.NeuralNetWizard => new NeuralNetWizardViewModel(),
                ViewModel.SimulationRunner => new SimulationRunnerViewModel(),
                _ => throw new ArgumentOutOfRangeException(nameof(viewModel), viewModel, null)
            };

        public virtual bool IsBusy { get; set; }

        public abstract void Abort();
    }
}