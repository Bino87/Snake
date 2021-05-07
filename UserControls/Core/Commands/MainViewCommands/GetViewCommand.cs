using System;
using UserControls.Core.Base;
using UserControls.Core.Commands.Base;
using UserControls.Core.Enums;
using UserControls.Models;

namespace UserControls.Core.Commands.MainViewCommands
{
    internal class GetViewCommand : RelayCommand<ViewModel>
    {
        internal GetViewCommand(MainDisplayHandlerViewModel viewModel) : base(model => viewModel.CurrentView = MainView.GetView(model))
        {
        }

        private GetViewCommand(MainDisplayHandlerViewModel viewModel, Predicate<ViewModel> canExecute) : base(model => viewModel.CurrentView = MainView.GetView(model), canExecute)
        {
        }
    }
}
