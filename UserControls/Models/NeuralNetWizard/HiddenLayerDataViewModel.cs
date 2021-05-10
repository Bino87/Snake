using System;
using System.ComponentModel;
using Network;
using Network.Enums;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;

namespace UserControls.Models.NeuralNetWizard
{
    public class HiddenLayerDataViewModel : Observable
    {
        private int _numberOfNodes = 1;
        private ActivationFunctionType _selectedActivationFunctionType;

        public int NumberOfNodes
        {
            get => _numberOfNodes;
            set => SetField(ref _numberOfNodes, value);
        }

        public ActivationFunctionType SelectedActivationFunctionType
        {
            get => _selectedActivationFunctionType;
            set => SetField(ref _selectedActivationFunctionType, value);
        }

        public static ActivationFunctionType[] ActivationFunctionTypes => Enum.GetValues<ActivationFunctionType>();

        public RelayCommand Remove { get; set; }

        public HiddenLayerDataViewModel(Action<HiddenLayerDataViewModel> action, PropertyChangedEventHandler onPropertyChanged)
        {
            Remove = new RelayCommand(() => action(this));
            PropertyChanged += onPropertyChanged;
        }

        public LayerInfo ToLayerInfo() => new(SelectedActivationFunctionType, NumberOfNodes);
    }
}