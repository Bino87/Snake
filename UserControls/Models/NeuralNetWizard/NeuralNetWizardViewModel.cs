﻿using UserControls.Core.Base;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models.NeuralNetWizard
{
    public class NeuralNetWizardNetworkTemplateListViewModel : Observable
    {

    }

    public class NeuralNetWizardViewModel : MainView
    {
        public NeuralNetDisplayViewModel NeuralNetDisplay { get; set; }
        public NeuralNetWizardSettingsViewModel NeuralNetWizardSettingsViewModel { get; set; }
        public NeuralNetWizardNetworkTemplateListViewModel NeuralNetWizardNetworkTemplateListViewModel { get; set; }

        public NeuralNetWizardViewModel()
        {
            NeuralNetDisplay = new NeuralNetDisplayViewModel();
            NeuralNetWizardSettingsViewModel = new NeuralNetWizardSettingsViewModel(NeuralNetDisplay);
            NeuralNetWizardNetworkTemplateListViewModel = new NeuralNetWizardNetworkTemplateListViewModel();
        }


        public override void Abort()
        {

        }
    }
}