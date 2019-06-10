using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wyd2.Client.ViewModel
{
    public class RelayCommand : ICommand
    {
        private Action _methodWithoutArgs;
        private Func<bool> _canExecuteWithoutArgs;

        private Func<object, bool> _canExecute;
        private Action<object> _method;

        public RelayCommand(Action<object> method, Func<object, bool> canExecute)
        {
            _method = method;
            _canExecute = canExecute;
        }

        public RelayCommand(Action method, Func<bool> canExecute)
        {
            _methodWithoutArgs = method;
            _canExecuteWithoutArgs = canExecute;
    }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return (bool)_canExecute?.Invoke(parameter);
            else if (_canExecuteWithoutArgs != null)
                return (bool)_canExecuteWithoutArgs?.Invoke();

            return true;
        }

        public void Execute(object parameter)
        {
            if (_method != null)
                _method(parameter);
            else _methodWithoutArgs?.Invoke();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

    }
}
