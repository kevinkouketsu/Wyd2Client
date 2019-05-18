using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Wyd2.Client.Model;
using Wyd2.Client.View;
using WYD2.Common.GameStructure;
using WYD2.Common.IncomingPacketStructure;
using WYD2.Network;

namespace Wyd2.Client.ViewModel
{
    public class PlayerViewModel : BaseViewModel
    {
        private ClientConnection Network { get; }
        private ClientControl Client { get; set; }

        private PlayerModel Player { get; }

        private MSelChar _selChar;
        private MSelChar SelChar
        {
            get => _selChar;
            set
            {
                _selChar = value;

                OnPropertyChanged();
            }
        }

        public TPlayerState State
        {
            get => Player.State;
            set
            {
                Player.State = value;

                OnPropertyChanged();
            }
        }

        private ICommand _createCharacter;
        public ICommand CreateCharacter
        {
            get
            {
                if (_createCharacter == null)
                {
                    _createCharacter = new BaseCommand(async (obj) =>
                    {
                        var context = new CreateCharacterViewModel();
                        var window = new CreateCharacterWindow()
                        {
                            DataContext = context
                        };

                        context.CreateCommand = new BaseCommand((obj) =>
                        {
                            int index = SelChar.Names.IndexOf(SelChar.Names.First(x => string.IsNullOrWhiteSpace(x.Name)));

                            Client.CreateCharacter(context.Name, index, (int)context.Class);
                        }, (obj) => true);

                        bool result = (bool)await DialogHost.Show(window, "RootDialog");
                    },
                    (obj) => (Player.State == TPlayerState.SelChar || Player.State == TPlayerState.Token) && SelChar.Names.Count(x => string.IsNullOrWhiteSpace(x.Name)) > 0);
                }

                return _createCharacter;
            }
        }

        public PlayerViewModel()
        {
            Player = new PlayerModel();

            Network = new ClientConnection("192.168.15.12", 8281);
            Network.OnSuccessfullConnect += this.Network_OnSuccessfullConnect;
            Network.OnSucessfullLogin += this.Network_OnSucessfullLogin;
            Network.OnTokenResponse += this.Network_OnTokenResponse;
            Network.OnReceiveUnknowPacket += this.Network_OnReceiveUnknowPacket;
            Network.OnRefreshCharList += this.Network_OnRefreshCharList;

            Network.Connect();
        }

        private void Network_OnRefreshCharList(object sender, MResendCharList e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);

            SelChar = e.SelChar;
            Messages.Add("A tela de seleção de personagens foi atualizada");
        }

        private void Network_OnTokenResponse(object sender, bool e)
        {
            //Messages.Add(e ? "Senha numérica correta" : "Senha numérica incorreta");
            Console.WriteLine(e ? "Senha numérica correta" : "Senha numérica incorreta");
            if (e)
            {
                //Messages.Add("Tentando logar no personagem 0");

                Player.State = TPlayerState.Token;
                //Client.EnterMob(0);
            }
        }

        private void Network_OnReceiveUnknowPacket(object sender, ushort e)
        {
            UnknowPackets.Add(e);
        }

        private void Network_OnSuccessfullConnect(object sender, EventArgs e)
        {
            Client = new ClientControl(Network);
            Client.Login("admin", "admin", 1001);
        }

        private void Network_OnSucessfullLogin(object sender, MLoginSuccessfulPacket e)
        {
            SelChar = e.SelChar;
            AccountName = e.AccName;

            Client.SendToken("1208", 0);

            Player.State = TPlayerState.SelChar;
        }

        public string AccountName
        {
            get => Player.Mob.Name.Value;
            set
            {
                Player.Mob.Name = new MMobName(value);

                OnPropertyChanged();
            }
        }

        public IList<MMobName> CharName
        {
            get
            {
                if (SelChar.Names == null)
                    return null;

                return SelChar.Names.ToList();
            }
        }

        public ObservableCollection<ushort> UnknowPackets { get; set; } = new ObservableCollection<ushort>();
        public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();
    }
}
