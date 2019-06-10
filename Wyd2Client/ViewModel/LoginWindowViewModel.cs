using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wyd2.Client.Model;
using Wyd2.Client.View;

namespace Wyd2.Client.ViewModel
{
    public class LoginWindowViewModel : BaseViewModel
    {
        private LoginWindowModel Model;

        public ObservableCollection<LoginWindowModel.ServerInfo> ServerList { get; set; } = new ObservableCollection<LoginWindowModel.ServerInfo>()
        {
            new LoginWindowModel.ServerInfo("Canal 01", "147.135.120.141"),
            new LoginWindowModel.ServerInfo("Canal 02", "147.135.120.148"),
            new LoginWindowModel.ServerInfo("Canal 03", "51.81.0.90"),
            new LoginWindowModel.ServerInfo("Canal 04", "51.81.0.91"),
            new LoginWindowModel.ServerInfo("Canal 05", "51.81.0.92"),
            new LoginWindowModel.ServerInfo("Canal 06", "51.81.0.93"),
            new LoginWindowModel.ServerInfo("Canal 07", "51.81.0.94"),
            new LoginWindowModel.ServerInfo("Canal 08", "51.81.0.95"),
        };

        public string Login
        {
            get => Model.Login;
            set
            {
                Model.Login = value;

                OnPropertyChanged();
            }
        }

        public string Token
        {
            get => Model.Token;
            set
            {
                Model.Token = value;

                OnPropertyChanged();
            }
        }

        public int SelectedServerIndex
        {
            get => Model.SelectedServerIndex;
            set
            {
                Model.SelectedServerIndex = value;

                OnPropertyChanged();
            }
        }

        public LoginWindowModel.ServerInfo SelectedServer { get; set; }

        public ICommand LoginCommand { get; }
        public LoginWindowViewModel()
        {
            Model = new LoginWindowModel();

            LoginCommand = new RelayCommand(DoLogin, (a) => true);
        }

        private async void DoLogin(object parameter)
        {
            if(SelectedServerIndex == -1)
            {
                await DialogHost.Show(new GameMessage("Selecione um canal"), "LoginWindowDialog");

                return;
            }
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
