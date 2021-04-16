using System;
using System.Windows.Input;

namespace UserControls.Core.Commands.Base
{
    public class RelayCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;

        public RelayCommand(Action execute) : this(execute, () => true)
        {

        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _execute != null && _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private Action<T> _execute;
        private Predicate<T> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<T> execute) : this(execute, x => true)
        {

        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is T p)
                return this._canExecute == null || this._canExecute(p);
            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter is T p)
                this._execute(p);
        }
    }
}
