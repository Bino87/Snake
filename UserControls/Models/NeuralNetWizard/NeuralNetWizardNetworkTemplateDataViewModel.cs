using System;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;

namespace UserControls.Models.NeuralNetWizard
{
    public record NetworkTemplateInfo(int Nodes, string ActivationFunction, string Layer);

    public class NeuralNetWizardNetworkTemplateDataViewModel : Observable
    {
        private NetworkTemplateDto _networkTemplateDto;

        public NetworkTemplateInfo[] NetworkTemplateInfos { get; private set; }

        public Guid Id => _networkTemplateDto.Id;
        public string Name => _networkTemplateDto.Name;

        public RelayCommand Delete { get; set; }
        public RelayCommand Modify { get; set; }

        public NeuralNetWizardNetworkTemplateDataViewModel(NetworkTemplateDto networkTemplateDto, int index, Action<Guid, int> onDelete, Action<NetworkTemplateDto> onModify)
        {
            _networkTemplateDto = networkTemplateDto;

            SetNetworkInfo(networkTemplateDto);

            Delete = new RelayCommand(() => onDelete(_networkTemplateDto.Id, index));
            Modify = new RelayCommand(() => onModify(_networkTemplateDto));
        }

        private void SetNetworkInfo(NetworkTemplateDto networkTemplateDto)
        {
            NetworkTemplateInfos = new NetworkTemplateInfo[networkTemplateDto.LayerSetup.Length];

            for (int i = 0; i < networkTemplateDto.LayerSetup.Length; i++)
            {
                if (i is 0)
                    NetworkTemplateInfos[i] = new NetworkTemplateInfo(networkTemplateDto.LayerSetup[i], "", "Input");
                else
                    NetworkTemplateInfos[i] = new NetworkTemplateInfo(networkTemplateDto.LayerSetup[i],
                        networkTemplateDto.ActivationFunctions[i - 1],
                        i < networkTemplateDto.LayerSetup.Length - 1 ? "Hidden" : "Output");
            }
        }

        public void Replace(NetworkTemplateDto networkTemplateDto)
        {
            _networkTemplateDto = networkTemplateDto;
            SetNetworkInfo(networkTemplateDto);
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(NetworkTemplateInfos));

        }

    }
}