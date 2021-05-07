using System;
using System.Collections.ObjectModel;
using System.Windows;
using Network;
using Network.Enums;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models.NeuralNetWizard
{
    public class NeuralNetWizardViewModel : MainView
    {
        private int _numberOfInputs;
        private int _numberOfOutputs;

        public ActivationFunctionType[] ActivationFunctionTypes => Enum.GetValues<ActivationFunctionType>();

        public NeuralNetDisplayViewModel NeuralNetDisplay { get; set; }

        public int NumberOfInputs
        {
            get => _numberOfInputs;
            set => SetField(ref _numberOfInputs, value);
        }

        public int NumberOfOutputs
        {
            get => _numberOfOutputs;
            set => SetField(ref _numberOfOutputs, value);
        }

        public ObservableCollection<HiddenLayerDataViewModel> HiddenLayers
        {
            get;
            set;
        }

        public RelayCommand AddLayer
        {
            get;
            set;
        }

        public ActivationFunctionType SelectedActivationFunctionType
        {
            get;
            set;
        }

        public RelayCommand CreateTemplate
        {
            get;
            set;
        }

        public NeuralNetWizardViewModel()
        {
            NeuralNetDisplay = new NeuralNetDisplayViewModel();
            HiddenLayers = new ObservableCollection<HiddenLayerDataViewModel>();
            AddLayer = new RelayCommand(OnAddLayer);
            CreateTemplate = new RelayCommand(OnCreateTemplate);

        }

        private void OnCreateTemplate()
        {
            if (Validate())
            {
                NetworkTemplate nt = new(NumberOfInputs, new LayerInfo(SelectedActivationFunctionType, NumberOfOutputs), GetLayerInfos());
                NeuralNetDisplay.Clear();
                NeuralNetDisplay.Initialize(nt);
            }
        }

        LayerInfo[] GetLayerInfos()
        {
            LayerInfo[] res = new LayerInfo[HiddenLayers.Count];

            for (int i = 0; i < HiddenLayers.Count; i++)
            {
                res[i] = HiddenLayers[i].ToLayerInfo();
            }

            return res;
        }

        private bool Validate()
        {
            if (NumberOfInputs <= 0)
            {
                MessageBox.Show("Input must have at least 1 node!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (NumberOfOutputs <= 0)
            {
                MessageBox.Show("Output must have at least 1 node!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            foreach (HiddenLayerDataViewModel layer in HiddenLayers)
            {
                if (layer.NumberOfNodes <= 0)
                {
                    MessageBox.Show("Layer must have at least 1 node!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

            }

            return true;
        }

        private void OnAddLayer()
        {
            HiddenLayers.Add(new HiddenLayerDataViewModel((item) => HiddenLayers.Remove(item)));
        }

        public override void Abort()
        {

        }
    }
}