using System;
using System.Data;
using System.Windows.Input;

namespace Visutron900PlusInterface.UserInterface.WpfTools
{
    public class BaseCommand : ICommand
    {
        private readonly Action<object> _executeActionWithParameter;
        private readonly Func<object, bool> _canExecuteFuncWithParameter;
        
        private readonly Action _executeAction;
        private readonly Func<bool> _canExecuteFunc;

        private readonly bool _parameterless = false;

        private bool _canExecute;

        public BaseCommand(Action executeAction, Func<bool> canExecuteFunc = null)
        {
            if (executeAction == null)
            {
                throw new NoNullAllowedException("executeAction should not be null");
            }
            _executeAction = executeAction;
            _canExecuteFunc = canExecuteFunc;
            _parameterless = true;
        }

        public BaseCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc = null)
        {
            if (executeAction == null)
            {
                throw new NoNullAllowedException("executeAction should not be null");
            }
            _executeActionWithParameter = executeAction;
            _canExecuteFuncWithParameter = canExecuteFunc;
        }

        public bool CanExecuteState
        {
            get
            {
                return !_parameterless || _canExecuteFunc();
            }
            set
            {
                _canExecute = value;
                RaiseCanExecuteChanged();
            }
        }

        public bool CanExecute(object parameter)
        {
            var canExecute = false;
            if (_canExecuteFunc == null)
            {
                canExecute = _canExecute;
            }
            else
            {
                canExecute = _parameterless ? _canExecuteFunc() : _canExecuteFuncWithParameter(parameter);
            }

            return canExecute;
        }

        public void Execute(object parameter)
        {
            if (_parameterless)
            {
                _executeAction();
            }
            else
            {
                _executeActionWithParameter(parameter);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged !=null)
            {
                CanExecuteChanged(this, new EventArgs());
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
