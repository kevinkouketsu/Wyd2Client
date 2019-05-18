using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wyd2.Client.ViewModel
{
    public class BaseCommand : ICommand
    {
        private Func<object, bool> _canExecute;
        private Action<object> _method;

        public event EventHandler CanExecuteChanged;

        public BaseCommand(Action<object> method, Func<object, bool> canExecute)
        {
            _method = method;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return (bool)_canExecute?.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _method.Invoke(parameter);
        }
    }
}
