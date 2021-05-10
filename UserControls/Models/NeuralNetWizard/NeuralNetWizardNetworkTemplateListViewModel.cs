using System;
using System.Collections.ObjectModel;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;

namespace UserControls.Models.NeuralNetWizard
{
    public class NeuralNetWizardNetworkTemplateDataViewModel : Observable
    {
        private readonly NetworkTemplateDto _networkTemplateDto;

        public int[] BiasCount { get; set; }

        public int[] WeightsCount { get; set; }

        public string[] ActivationFunctions { get; set; }

        public int Layers { get; set; }

        public int OutputCount { get; set; }

        public int InputCount { get; set; }

        public int[] LayerSetup { get; set; }
        public string Name => _networkTemplateDto.Name;

        public RelayCommand Delete { get; set; }
        public RelayCommand Modify { get; set; }

        public NeuralNetWizardNetworkTemplateDataViewModel(NetworkTemplateDto networkTemplateDto, int index, Action<Guid, int> onDelete, Action<NetworkTemplateDto> onModify)
        {
            _networkTemplateDto = networkTemplateDto;
            Delete = new RelayCommand(() => onDelete(_networkTemplateDto.Id, index));
            Modify = new RelayCommand(() => onModify(_networkTemplateDto));
        }


    }

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