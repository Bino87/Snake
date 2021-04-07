using System;
using System.Windows.Input;

namespace UserControls.Models.Commands
{
    public class RelayCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        public RelayCommand(Action execute) : this(execute, () => true)
        {

        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return execute != null && canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private Action<T> execute;
        private Predicate<T> canExecute;

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
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is T p)
                return this.canExecute == null || this.canExecute(p);
            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter is T p)
                this.execute(p);
        }
    }
}
