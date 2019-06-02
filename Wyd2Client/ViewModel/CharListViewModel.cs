using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wyd2.Client.Model;
using Wyd2.Client.View;
using WYD2.Common.GameStructure;
using WYD2.Control;

namespace Wyd2.Client.ViewModel
{
    public class CharListViewModel : BaseViewModel
    {
        private CharListModel Model { get; }

        public ICommand CreateCharacterCommand { get; }
        public ICommand DeleteCharacterCommand { get; }
        public ICommand EnterCharacterCommand { get; }

        public TPlayerState State
        {
            get => Model.State;
            set
            {
                Model.State = value;

                OnPropertyChanged();
            }
        }

        public IList<MobName> CharName
        {
            get
            {
                if (SelChar.Names == null)
                    return null;

                return SelChar.Names.ToList();
            }
        }

        public MSelChar SelChar
        {
            get => Model.SelChar;
            set
            {
                Model.SelChar = value;

                OnPropertyChanged();
            }
        }

        public string SelectedCharlistCharacter
        {
            get => Model.SelectedCharlistCharacter;
            set
            {
                Model.SelectedCharlistCharacter = value;

                OnPropertyChanged();
            }
        }

        private int SelectedCharacterIndex
        {
            get => Model.SelChar.Names.IndexOf(Model.SelChar.Names.First(x => x.Name == SelectedCharlistCharacter));
        }

        private ClientControl Client { get; }

        public CharListViewModel(ClientControl client, MSelChar selChar)
        {
            Model = new CharListModel();

            State = TPlayerState.Token;

            SelChar = selChar;
            CreateCharacterCommand = new RelayCommand(CreateCharacter, CanCreateCharacter);
            DeleteCharacterCommand = new RelayCommand(DeleteCharacter, CanDeleteCharacter);
            EnterCharacterCommand = new RelayCommand(EnterCharacter, CanEnterCharacter);

            Client = client;
        }

        private async void DeleteCharacter()
        {
            DeleteCharacterViewModel context = new DeleteCharacterViewModel(DeleteCharacter);
            DeleteCharacterWindow window = new DeleteCharacterWindow()
            {
                DataContext = context
            };

            await DialogHost.Show(window, "CharlistDialog");

            void DeleteCharacter(object parameter)
            {
                var passwordContainer = parameter as IHavePassword;
                if (passwordContainer == null)
                    return;

                var password = passwordContainer.Password;

                int index = SelectedCharacterIndex;
                if (index == -1)
                    return;

                Client.DeleteCharacter(SelectedCharlistCharacter, index, password);
            }
        }

        private bool CanDeleteCharacter()
        {
            return State == TPlayerState.Token && !string.IsNullOrWhiteSpace(SelectedCharlistCharacter);
        }

        private async void CreateCharacter()
        {
            var context = new CreateCharacterViewModel();
            var window = new CreateCharacterWindow(context);

            context.CreateCommand = new RelayCommand(() =>
            {
                int index = SelChar.Names.IndexOf(SelChar.Names.First(x => string.IsNullOrWhiteSpace(x.Name)));

                Client.CreateCharacter(context.Name, index, (int)context.Class);
            }, () => true);

            await DialogHost.Show(window, "CharlistDialog");
        }

        private bool CanCreateCharacter()
        {
            return State == TPlayerState.Token && SelChar.Names.Count(x => string.IsNullOrWhiteSpace(x.Name)) > 0;
        }

        private void EnterCharacter()
        {
            if (State == TPlayerState.Token)
            {
                Client.EnterMob(SelectedCharacterIndex);

                return;
            }

            Client.CharLogout();
        }

        private bool CanEnterCharacter()
        {
            if (State == TPlayerState.Empty || State == TPlayerState.Hello || State == TPlayerState.SelChar)
                return false;

            if (State == TPlayerState.Token && !string.IsNullOrWhiteSpace(SelectedCharlistCharacter))
                return true;

            if (State == TPlayerState.Play)
                return true;

            return false;
        }

    }
}
