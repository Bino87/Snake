using DataAccessLibrary.DataTransferObjects.NetworkDTOs;

namespace UserControls.Core.Objects.NeuralNetWizard
{
    public class NetworkTemplateSavedEventArgs
    {
        public NetworkTemplateDto Template { get; init; }
        public bool IsModified { get; init; }
        public bool IsInserted { get; init; }

        public NetworkTemplateSavedEventArgs(NetworkTemplateDto template, bool isModified, bool isInserted)
        {
            Template = template;
            IsModified = isModified;
            IsInserted = isInserted;
        }
    }
}
