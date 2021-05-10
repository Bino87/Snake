using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using Network;
using Network.Enums;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models.NeuralNetWizard
{
    public class NeuralNetWizardViewModel : MainView
    {
        private int _numberOfInputs = 1;
        private int _numberOfOutputs = 1;
        private ActivationFunctionType _selectedActivationFunctionForOutput;
        private string _name;
        private bool _overrideName;

        public bool OverrideName
        {
            get => _overrideName;
            set => SetField(ref _overrideName, value);
        }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }
        public static ActivationFunctionType[] ActivationFunctionTypes => Enum.GetValues<ActivationFunctionType>();

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
            get => _selectedActivationFunctionForOutput;
            set => SetField(ref _selectedActivationFunctionForOutput, value);
        }

        public RelayCommand CreateTemplate
        {
            get;
            set;
        }

        public RelayCommand SaveToDataBase
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
            SaveToDataBase = new RelayCommand(OnSaveToDataBase);
            PropertyChanged += OnPropertyChanged;
            HiddenLayers.CollectionChanged += HiddenLayersOnCollectionChanged;
        }

        private void HiddenLayersOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            TryGenerateName();
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Name)))
                return;
            TryGenerateName();
        }

        private void OnSaveToDataBase()
        {
            NetworkTemplateAccess nta = new NetworkTemplateAccess();
            NeuralNetDisplay.NetworkTemplate.Id = nta.Insert(NeuralNetDisplay.NetworkTemplate.ToDataTransferObject());
        }

        private void OnCreateTemplate()
        {
            NetworkTemplate nt = new(Name, NumberOfInputs, new LayerInfo(SelectedActivationFunctionType, NumberOfOutputs), GetLayerInfos());
            NeuralNetDisplay.Initialize(nt);
        }

        private void TryGenerateName()
        {
            if (OverrideName)
                return;

            object[] arr = new object[3 + HiddenLayers.Count * 2];
            arr[0] = NumberOfInputs;

            for (int i = 0; i < HiddenLayers.Count; i++)
            {
                arr[1 + i * 2] = HiddenLayers[i].NumberOfNodes;
                arr[1 + i * 2 + 1] = HiddenLayers[i].SelectedActivationFunctionType;
            }

            arr[^2] = NumberOfOutputs;
            arr[^1] = SelectedActivationFunctionType;

            Name = string.Join("_", arr);
        }

        private LayerInfo[] GetLayerInfos()
        {
            LayerInfo[] res = new LayerInfo[HiddenLayers.Count];

            for (int i = 0; i < HiddenLayers.Count; i++)
            {
                res[i] = HiddenLayers[i].ToLayerInfo();
            }

            return res;
        }

        private void OnAddLayer()
        {
            HiddenLayers.Add(new HiddenLayerDataViewModel(item => HiddenLayers.Remove(item), OnPropertyChanged));
        }

        public override void Abort()
        {

        }
    }
}