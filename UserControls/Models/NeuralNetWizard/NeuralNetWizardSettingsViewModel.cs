using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Commons.Extensions;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using Network;
using Network.Enums;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;
using UserControls.Core.Objects.NeuralNetWizard;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models.NeuralNetWizard
{
    public class NeuralNetWizardSettingsViewModel : Observable
    {

        private int _numberOfInputs = 1;
        private int _numberOfOutputs = 1;
        private ActivationFunctionType _selectedActivationFunctionForOutput;
        private string _name;
        private bool _overrideName;
        private readonly NeuralNetDisplayViewModel _nnDisplay;
        public EventHandler<NetworkTemplateSavedEventArgs> NetworkTemplateAdded { get; set; }

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

        public ActivationFunctionType SelectedActivationFunctionType
        {
            get => _selectedActivationFunctionForOutput;
            set => SetField(ref _selectedActivationFunctionForOutput, value);
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

        public RelayCommand Preview
        {
            get;
            set;
        }

        public RelayCommand NewTemplate
        {
            get;
            set;
        }

        public RelayCommand SaveToDataBase
        {
            get;
            set;
        }

        public NeuralNetWizardSettingsViewModel(NeuralNetDisplayViewModel nnDisplay)
        {
            _nnDisplay = nnDisplay;
            HiddenLayers = new ObservableCollection<HiddenLayerDataViewModel>();
            AddLayer = new RelayCommand(OnAddLayer);
            Preview = new RelayCommand(OnPreview);
            NewTemplate = new RelayCommand(OnNewTemplate);
            SaveToDataBase = new RelayCommand(OnSaveToDataBase);
            PropertyChanged += OnPropertyChanged;
            HiddenLayers.CollectionChanged += HiddenLayersOnCollectionChanged;
            TryGenerateName();
        }

        private void OnPreview()
        {
            OnPreview(_nnDisplay?.NetworkTemplate?.Id);
        }

        private void OnNewTemplate()
        {
            _nnDisplay.Clear();
            HiddenLayers.Clear();

            NumberOfInputs = 1;
            NumberOfOutputs = 1;
            SelectedActivationFunctionType = ActivationFunctionType.BinaryStep;
            OverrideName = false;
            TryGenerateName();
        }

        private void HiddenLayersOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            TryGenerateName();
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(Name))
                return;
            TryGenerateName();
        }

        private void OnSaveToDataBase()
        {
            NetworkTemplateAccess nta = new();

            NetworkTemplateSavedEventArgs eventArgs;

            if (_nnDisplay.NetworkTemplate?.Id == default)
            {
                OnPreview(_nnDisplay?.NetworkTemplate?.Id);
                NetworkTemplateDto dto = _nnDisplay.NetworkTemplate.ToDataTransferObject();
                _nnDisplay.NetworkTemplate.Id = nta.Insert(dto);
                eventArgs = new NetworkTemplateSavedEventArgs(dto, false, true);
            }
            else
            {
                OnPreview(_nnDisplay?.NetworkTemplate?.Id);
                NetworkTemplateDto dto = _nnDisplay.NetworkTemplate.ToDataTransferObject();
                nta.Update(dto);
                eventArgs = new NetworkTemplateSavedEventArgs(dto, true, false);
            }

            NetworkTemplateAdded?.Invoke(this, eventArgs);
        }

        private void OnPreview(Guid? id)
        {
            NetworkTemplate nt = id == default
                ? new NetworkTemplate(Name, NumberOfInputs, new LayerInfo(SelectedActivationFunctionType, NumberOfOutputs), GetLayerInfos())
                : new NetworkTemplate(id.Value, Name, NumberOfInputs, new LayerInfo(SelectedActivationFunctionType, NumberOfOutputs), GetLayerInfos());

            _nnDisplay.Initialize(nt);
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

        public void SetToModify(NetworkTemplateDto dto)
        {
            OverrideName = true;
            Name = dto.Name;
            NumberOfInputs = dto.InputCount;
            NumberOfOutputs = dto.OutputCount;
            ActivationFunctionType[] activationFunc = dto.ActivationFunctions.FromStringArray<ActivationFunctionType>();
            SelectedActivationFunctionType = activationFunc[^1];

            HiddenLayers.Clear();

            for (int i = 0; i < activationFunc.Length - 1; i++)
            {
                HiddenLayers.Add(new HiddenLayerDataViewModel(item => HiddenLayers.Remove(item), OnPropertyChanged));
                HiddenLayers[i].NumberOfNodes = dto.LayerSetup[i];
                HiddenLayers[i].SelectedActivationFunctionType = activationFunc[i];
            }

            OnPreview(dto.Id);
        }
    }
}