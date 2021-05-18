using System;
using System.Collections.ObjectModel;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using Network;
using UserControls.Core.Base;
using UserControls.Core.Objects.NeuralNetWizard;

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

            neuralNetWizardSettingsViewModel.NetworkTemplateAdded += OnNetworkTemplateAdded;
        }

        private void OnNetworkTemplateAdded(object? sender, NetworkTemplateSavedEventArgs e)
        {
            if(e.IsInserted)
                NetworkTemplates.Add(new NeuralNetWizardNetworkTemplateDataViewModel(e.Template, NetworkTemplates.Count, OnDelete, OnModify));
            else if (e.IsModified)
            {
                foreach (NeuralNetWizardNetworkTemplateDataViewModel data in NetworkTemplates)
                {
                    if (data.Id != e.Template.Id) 
                        continue;
                    data.Replace(e.Template);
                    break;
                }
            }
        }


        private void OnModify(NetworkTemplateDto dto)
        {
            _neuralNetWizardSettingsViewModel.SetToModify(dto);
        }

        private void OnDelete(Guid id, NeuralNetWizardNetworkTemplateDataViewModel item)
        {
            NetworkTemplateAccess nta = new();
            nta.DeleteById(id);
            NetworkTemplates.Remove(item);
        }
    }
}