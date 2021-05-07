﻿using System;
using System.Collections.ObjectModel;
using Network;
using Network.Enums;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models.NeuralNetWizard
{
    public class NeuralNetWizardViewModel : MainView
    {
        private int _numberOfInputs  = 1;
        private int _numberOfOutputs = 1;

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
            NetworkTemplate nt = new(NumberOfInputs, new LayerInfo(SelectedActivationFunctionType, NumberOfOutputs), GetLayerInfos());
            NeuralNetDisplay.Initialize(nt);
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
            HiddenLayers.Add(new HiddenLayerDataViewModel((item) => HiddenLayers.Remove(item)));
        }

        public override void Abort()
        {

        }
    }
}