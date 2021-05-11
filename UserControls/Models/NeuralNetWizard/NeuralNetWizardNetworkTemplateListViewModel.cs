using System;
using System.Collections.ObjectModel;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using UserControls.Core.Base;

namespace UserControls.Models.NeuralNetWizard
{
    public class NeuralNetWizardNetworkTemplateListViewModel : Observable
    {
        private readonly NeuralNetWizardSettingsViewModel _neuralNetWizardSettingsViewModel;
        public ObservableCollection<NeuralNetWizardNetworkTemplateDataViewModel> NetworkTemplates { get; set; }

        public NeuralNetWizardNetworkTemplateListViewModel(NeuralNetWizardSettingsViewModel neuralNetWizardSettingsViewModel)
        {
            _neuralNetWizardSettingsViewModel = neuralNetWizardSettingsViewModel;
            NetworkTemplates = new ObservableCollection<NeuralNetWizardNetworkTemplateDataViewModel>();
            NetworkTemplateAccess nta = new();

            foreach (NetworkTemplateDto dto in nta.GetAll())
            {
                NetworkTemplates.Add(new NeuralNetWizardNetworkTemplateDataViewModel(dto, NetworkTemplates.Count, OnDelete, OnModify));
            }
        }

        private void OnModify(NetworkTemplateDto dto)
        {
            _neuralNetWizardSettingsViewModel.SetToModify(dto);
        }

        private void OnDelete(Guid id, int index)
        {
            NetworkTemplateAccess nta = new NetworkTemplateAccess();
            nta.DeleteById(id);
            NetworkTemplates.RemoveAt(index);
        }
    }
}