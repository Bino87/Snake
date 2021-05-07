using Commons.Extensions;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;
using UserControls.Core.Enums;
using UserControls.Models;

namespace UserControls.Core.Commands.MainViewCommands
{
    internal class GetViewCommand : RelayCommand<ViewModel>
    {
        internal GetViewCommand(MainDisplayHandlerViewModel viewModel) : base(model => TryExecute(viewModel, model))
        {
        }

        private static void TryExecute(MainDisplayHandlerViewModel viewModel, ViewModel model)
        {
            if (viewModel.CurrentView.IsNotNull() && viewModel.CurrentView.IsBusy )
            {
                viewModel.CurrentView.Abort();
            }

            viewModel.CurrentView = MainView.GetView(model);
        }
    }
}
