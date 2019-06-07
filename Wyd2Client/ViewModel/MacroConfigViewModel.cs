using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyd2.Client.Model;

namespace Wyd2.Client.ViewModel
{
    public class MacroConfigViewModel : BaseViewModel
    {
        private MacroConfigModel Model = new MacroConfigModel();

        public bool IsFixed
        {
            get => Model.IsFixed;
            set
            {
                Model.IsFixed = value;

                OnPropertyChanged();
            }
        }

        public bool IsContinous
        {
            get => Model.IsContinous;
            set
            {
                Model.IsContinous = value;

                OnPropertyChanged();
            }
        }
    }
}
