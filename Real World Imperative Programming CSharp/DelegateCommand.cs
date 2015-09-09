using System;
using System.Windows.Input;

namespace RealWorldImperativeProgrammingCSharp
{
    public class DelegateCommand : ICommand
    {
        private readonly Action executeAction;
        private readonly Func<bool> canExecute;

        public DelegateCommand(Action executeAction)
            : this(executeAction, () => true)
        {
        }

        public DelegateCommand(Action executeAction, Func<bool> canExecute)
        {
            this.executeAction = executeAction;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            executeAction();
        }

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }
    }
}
