using System;
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
}