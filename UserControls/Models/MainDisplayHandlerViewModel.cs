using UserControls.Core.Base;
using UserControls.Core.Commands.Base;
using UserControls.Core.Commands.MainViewCommands;
using UserControls.Core.Enums;

namespace UserControls.Models
{
    public class MainDisplayHandlerViewModel : Observable
    {
        private MainView _currentView;
        public MainView CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }


        public RelayCommand<ViewModel> ChangeContent
        {
            get;
        }

        public MainDisplayHandlerViewModel()
        {
            ChangeContent = new GetViewCommand(this);
        }

    }
}