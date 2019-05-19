using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wyd2.Client.ViewModel
{
    public interface IHavePassword
    {
        string Password { get; }
    }

    public class DeleteCharacterViewModel : BaseViewModel
    {
        public ICommand ConfirmDeleteCommand { get; }

        public DeleteCharacterViewModel(Action<object> confirmDelete)
        {
            ConfirmDeleteCommand = new RelayCommand(confirmDelete, null);
        }
    }
}
