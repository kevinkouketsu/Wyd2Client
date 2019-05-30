using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Wyd2.Client.Model;
using Wyd2.Client.View;
using WYD2.Common;
using WYD2.Common.GameStructure;
using WYD2.Common.IncomingPacketStructure;
using WYD2.Common.Utility;
using WYD2.Network;

namespace Wyd2.Client.ViewModel
{
    public class PlayerViewModel : BaseViewModel
    {
        #region Public Properties

        public string SelectedCharlistCharacter
        {
            get => Player.SelectedCharlistCharacter;
            set
            {
                Player.SelectedCharlistCharacter = value;

                OnPropertyChanged();
            }
        }

        public MSelChar SelChar
        {
            get => Player.SelChar;
            set
            {
                Player.SelChar = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(CharName));
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

        public string AccountName
        {
            get => Player.Mob.Name;
            set
            {
                Player.Mob.Name = value;

                OnPropertyChanged();
            }
        }

        public MainWindowModel Player { get; }

        public MMobCore Mob
        {
            get => Player.Mob;
            set
            {
                Player.Mob = value;
                FinalScore = value.FinalScore;

                OnPropertyChanged(nameof(Name));
            }
        }

        public MPosition Position
        {
            get => Player.Position;
            set
            {
                Player.Position = value;

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

        #region Character information

        public string Name
        {
            get => Player.Mob.Name;
        }

        public MScore FinalScore
        {
            get => Player.Mob.FinalScore;
            set
            {
                Player.Mob.FinalScore = value;

                OnPropertyChanged(nameof(Level));
                OnPropertyChanged(nameof(MaxHp));
                OnPropertyChanged(nameof(CurrentHp));
            }
        }

        public int Level
        {
            get => Player.Mob.FinalScore.Level + 1;
        }

        public int MaxHp
        {
            get => Player.Mob.FinalScore.MaxHp;
            set
            {
                Player.Mob.FinalScore.MaxHp = value;

                OnPropertyChanged();
            }
        }

        public int CurrentHp
        {
            get => Player.Mob.FinalScore.CurrHp;
            set
            {
                Player.Mob.FinalScore.CurrHp = value;

                OnPropertyChanged();
            }
        }

        #endregion
        public ObservableCollection<ushort> UnknowPackets { get; set; } = new AsyncObservableCollection<ushort>();
        public ObservableCollection<TMessage> Messages { get; set; } = new AsyncObservableCollection<TMessage>();
        public ObservableCollection<MobModel> Mobs { get; set; } = new AsyncObservableCollection<MobModel>();

        public ICommand CreateCharacterCommand { get; }
        public ICommand DeleteCharacterCommand { get; }
        public ICommand EnterCharacterCommand { get; }
        public ICommand MovementCommand { get; }

        #endregion

        #region Private Properties

        private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        private ClientConnection Network { get; }
        private ClientControl Client { get; set; }

        private int SelectedCharacterIndex
        {
            get => SelChar.Names.IndexOf(SelChar.Names.First(x => x.Name == SelectedCharlistCharacter));
        }

        #endregion

        #region Constructor

        public PlayerViewModel()
        {
            Player = new MainWindowModel();

            Network = new ClientConnection("51.81.0.90", 8281);

            // Commands 
            CreateCharacterCommand = new RelayCommand(CreateCharacter, CanCreateCharacter);
            DeleteCharacterCommand = new RelayCommand(DeleteCharacter, CanDeleteCharacter);
            EnterCharacterCommand = new RelayCommand(EnterCharacter, CanEnterCharacter);
            MovementCommand = new RelayCommand(Movement, CanMovement);

            Network.OnSuccessfullConnect += this.Network_OnSuccessfullConnect;
            Network.OnReceiveSucessfullLogin += this.Network_OnSucessfullLogin;
            Network.OnReceiveTokenResponse += this.Network_OnTokenResponse;
            Network.OnReceiveUnknowPacket += this.Network_OnReceiveUnknowPacket;
            Network.OnReceiveRefreshCharList += this.Network_OnRefreshCharList;
            Network.OnReceiveCreateCharacterError += this.Network_OnReceiveCreateCharacterError;
            Network.OnReceiveDeleteCharacterError += this.Network_OnReceiveDeleteCharacterError;
            Network.OnReceiveCharToWorld += this.Network_OnReceiveCharToWorld;
            Network.OnReceiveGameMessage += this.Network_OnReceiveGameMessage;
            Network.OnReceiveCreateMob += this.Network_OnReceiveCreateMob;
            Network.OnReceiveChatMessage += this.Network_OnReceiveChatMessage;
            Network.OnReceiveDeleteMob += this.Network_OnReceiveDeleteMob;
            Network.OnReceiveMovement += this.Network_OnReceiveMovement;
            Network.OnReceiveCharLogoutSignal += (sender, args) => State = TPlayerState.Token;

            Network.Connect();

            Console.WriteLine(Marshal.SizeOf(typeof(MMobCore)));
            Console.WriteLine(Marshal.SizeOf(typeof(MScore)));
        }

        #endregion

        #region Commands 

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

        private async void CreateCharacter()
        {
            var context = new CreateCharacterViewModel();
            var window = new CreateCharacterWindow(context);

            context.CreateCommand = new RelayCommand(() =>
            {
                int index = SelChar.Names.IndexOf(SelChar.Names.First(x => string.IsNullOrWhiteSpace(x.Name)));

                Client.CreateCharacter(context.Name, index, (int)context.Class);
            }, () => true);

            await DialogHost.Show(window, "RootDialog");
        }

        private bool CanCreateCharacter()
        {
            return Player.State == TPlayerState.Token && SelChar.Names.Count(x => string.IsNullOrWhiteSpace(x.Name)) > 0;
        }

        private async void DeleteCharacter()
        {
            DeleteCharacterViewModel context = new DeleteCharacterViewModel(DeleteCharacter);
            DeleteCharacterWindow window = new DeleteCharacterWindow()
            {
                DataContext = context
            };

            await DialogHost.Show(window, "RootDialog");

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
            return Player.State == TPlayerState.Token && !string.IsNullOrWhiteSpace(SelectedCharlistCharacter);
        }

        private bool CanMovement(object arg)
        {
            return true;
        }

        private void Movement(object obj)
        {
            string cmd = obj as string;
            if (cmd == null)
                throw new Exception();

            MPosition pos = Position;
            if (cmd == "Up")
                pos.Y++;
            else if (cmd == "Down")
                pos.Y--;
            else if (cmd == "Right")
                pos.X++;
            else
                pos.X--;

            Client.Movement(Player.ClientId, Player.Position, pos, 0, Player.Mob.FinalScore.MovementSpeed);

            Messages.Add(new TMessage($"Movimentado de {Player.Position } para { pos } ", TMessage.NormalColor));
            Position = pos;
        }

        #endregion

        #region Events from Server 
        private void Network_OnReceiveCreateCharacterError(object sender, EventArgs e)
        {
            _synchronizationContext.Send(async (a) =>
            {
                await DialogHost.Show(new GameMessage("Nome em uso ou existem caracteres inválidos"), "CreateCharacterDialog");
            }, e);
        }

        private void Network_OnRefreshCharList(object sender, MResendCharListPacket e)
        {
            _synchronizationContext.Send((a) =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            }, e);

            SelChar = e.SelChar;
            Messages.Add(new TMessage("A tela de seleção de personagens foi atualizada", TMessage.SystemColor));
        }

        private void Network_OnTokenResponse(object sender, bool e)
        {
            _synchronizationContext.Send((a) =>
            {
                Messages.Add(new TMessage(e ? "Senha numérica correta" : "Senha numérica incorreta", TMessage.SystemColor));
            }, e);

            if (e)
                Player.State = TPlayerState.Token;
        }

        private void Network_OnReceiveUnknowPacket(object sender, ushort e)
        {
            UnknowPackets.Add(e);
        }

        private void Network_OnSuccessfullConnect(object sender, EventArgs e)
        {
            Client = new ClientControl(Network);
            Client.Login("shepher", "kevin123", 762);
        }

        private void Network_OnSucessfullLogin(object sender, MLoginSuccessfulPacket e)
        {
            SelChar = e.SelChar;
            AccountName = e.AccName;

            PacketSecurity.HashTable = e.HashKeyTable;
            Client.SendToken("1208", 0);

            Player.State = TPlayerState.SelChar;
        }

        private void Network_OnReceiveDeleteCharacterError(object sender, EventArgs e)
        {
            _synchronizationContext.Send(async (a) =>
            {
                await DialogHost.Show(new GameMessage("Falha ao deletar personagem"), "DeleteCharacterDialog");
            }, e);
        }

        private void Network_OnReceiveGameMessage(object sender, string e)
        {
            Console.WriteLine(e);
        }

        private void Network_OnReceiveCharToWorld(object sender, MCharToWorldPacket e)
        {
            Mob = e.Mob;
            Player.ClientId = e.ClientIndex;
            Position = e.Position;

            State = TPlayerState.Play;
        }

        private void Network_OnReceiveCreateMob(object sender, MCreateMobPacket e)
        {
            if(e.Index == Player.ClientId)
            {
                Position = e.Position;

                for(int i = 0; i < GameBasics.MAXL_AFFECT; i++)
                {
                    Player.Affects[i].Type = e.Affect[i].Index;
                    Player.Affects[i].Time = e.Affect[i].Time;
                }

                return;
            }

            if (Mobs.Count(x => x.Index == e.Index) > 0)
                return;

            _synchronizationContext.Send((a) =>
            {
                Mobs.Add(new MobModel(e.Name, e.Index)
                {
                    Score = e.Score,
                    Position = e.Position,
                });
            }, null);
        }

        private void Network_OnReceiveDeleteMob(object sender, MSignalValuePacket e)
        {
            var mobs = Mobs.Where(x => x.Index == e.Header.ClientId);
            if (mobs.Count() <= 0)
                return;

            _synchronizationContext.Send((a) =>
            {
                Mobs.Remove(mobs.First());
            }, e);
        }

        private void Network_OnReceiveChatMessage(object sender, MChatMessagePacket e)
        {
            _synchronizationContext.Send((a) =>
            {
                var mobs = Mobs.Where(x => x.Index == e.Header.ClientId);
                if (mobs.Count() <= 0)
                    return;

                var mob = Mobs.First();
                Messages.Add(new TMessage($"[{ mob.Name }]> { e.Message }", TMessage.NormalColor));
            }, e);
        }

        private void Network_OnReceiveMovement(object sender, MMovePacket e)
        {
            int index = e.Header.ClientId;
            if(index == Player.ClientId)
            {
                Position = e.Destiny;

                return;
            }

            var mob = Mobs.ById(index);
            if (mob == null)
                return;

            mob.Position = e.Destiny;
        }
        #endregion
    }
}
