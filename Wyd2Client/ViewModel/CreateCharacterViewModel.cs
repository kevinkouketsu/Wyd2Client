using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wyd2.Client.Model;
using WYD2.Common.GameStructure;

namespace Wyd2.Client.ViewModel
{
    public class CreateCharacterViewModel : BaseViewModel
    {
        private CreateCharacterModel Model = new CreateCharacterModel();

        public ICommand CreateCommand { get; set; }

        public string Name
        {
            get => Model.Name;
            set
            {
                Model.Name = value;

                OnPropertyChanged();
            }
        }

        public ECharClass Class
        {
            get => Model.Class;
            set => Model.Class = value;
        }

        public bool IsTK
        {
            get => Class == ECharClass.TK;
            set
            {
                Class = ECharClass.TK;

                OnPropertyChanged(nameof(IsFM));
                OnPropertyChanged(nameof(IsHT));
                OnPropertyChanged(nameof(IsTK));
                OnPropertyChanged(nameof(IsBM));
            }
        }
        public bool IsBM
        {
            get => Class == ECharClass.BM;
            set
            {
                Class = ECharClass.BM;

                OnPropertyChanged(nameof(IsFM));
                OnPropertyChanged(nameof(IsHT));
                OnPropertyChanged(nameof(IsTK));
                OnPropertyChanged(nameof(IsBM));
            }
        }
        public bool IsHT
        {
            get => Class == ECharClass.HT;
            set
            {
                Class = ECharClass.HT;

                OnPropertyChanged(nameof(IsFM));
                OnPropertyChanged(nameof(IsHT));
                OnPropertyChanged(nameof(IsTK));
                OnPropertyChanged(nameof(IsBM));
            }
        }
        public bool IsFM
        {
            get => Class == ECharClass.FM;
            set
            {
                Class = ECharClass.FM;

                OnPropertyChanged(nameof(IsFM));
                OnPropertyChanged(nameof(IsHT));
                OnPropertyChanged(nameof(IsTK));
                OnPropertyChanged(nameof(IsBM));
            }
        }
    }
}
