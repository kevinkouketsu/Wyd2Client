using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyd2.Client.Model;

namespace Wyd2.Client.ViewModel
{
    public class GameMessageViewModel : BaseViewModel
    {
        private GameMessageModel Model = new GameMessageModel();

        public string Message
        {
            get => Model.Message;
            set
            {
                Model.Message = value;

                OnPropertyChanged();
            }
        }
    }
}
