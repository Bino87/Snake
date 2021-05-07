using System;
using Network;
using Network.Enums;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;

namespace UserControls.Models.NeuralNetWizard
{
    public class HiddenLayerDataViewModel : Observable
    {
        private int _numberOfNodes = 1;

        public int NumberOfNodes
        {
            get => _numberOfNodes;
            set => SetField(ref _numberOfNodes, value);
        }

        public ActivationFunctionType SelectedActivationFunctionType
        {
            get;
            set;
        }

        public static ActivationFunctionType[] ActivationFunctionTypes => Enum.GetValues<ActivationFunctionType>();

        public RelayCommand Remove { get; set; }

        public HiddenLayerDataViewModel(Action<HiddenLayerDataViewModel> action)
        {
            Remove = new RelayCommand(() => action(this));
        }

        public LayerInfo ToLayerInfo() => new(SelectedActivationFunctionType, NumberOfNodes);
    }
}