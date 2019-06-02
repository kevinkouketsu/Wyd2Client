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
using WYD2.Control;
using WYD2.Control.System;

namespace Wyd2.Client.ViewModel
{
    public class PlayerViewModel : BaseViewModel
    {
        #region Public Properties

        public MPlayer Player { get; }

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

        public bool IsPhysical
        {
            get => Model.IsPhysical;
            set
            {
                Model.IsPhysical = value;

                Macro = new PhysicalMacro(Player, Mobs);
                Macro.OnAttackMob += (a, b) =>
                {
                    Client.SendPacket(b);
                };

                OnPropertyChanged();
            }
        }

        public bool IsMagical
        {
            get => Model.IsMagical;
            set
            {
                Model.IsMagical = value;

                OnPropertyChanged();
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

        public string Message
        {
            get => Model.Message;
            set
            {
                Model.Message = value;

                OnPropertyChanged();
            }
        }

        #endregion
        public ObservableCollection<ushort> UnknowPackets { get; set; } = new AsyncObservableCollection<ushort>();
        public ObservableCollection<TMessage> Messages { get; set; } = new AsyncObservableCollection<TMessage>();
        public ObservableCollection<MMob> Mobs { get; set; } = new AsyncObservableCollection<MMob>();

        public MainWindowModel Model { get; } = new MainWindowModel();

        public ICommand MovementCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand EnterCommand { get; }
        #endregion

        #region Private Properties

        private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        private ClientConnection Network { get; }
        private ClientControl Client { get; set; }

        private DispatcherTimer Timer { get; }

        private MacroSystem Macro { get; set; }
        #endregion

        #region Constructor

        public PlayerViewModel()
        {
            Player = new MPlayer();

            W2Objects.ItemList = ConfigReader.ReadItemList("ItemList.csv", "ItemEffect.h");

            Network = new ClientConnection("51.81.0.90", 8281);
            Client = new ClientControl(Network);

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1500);
            Timer.Tick += Timer_Tick;
            Timer.Start();

            // Commands 
            MovementCommand = new RelayCommand(Movement, CanMovement);

            SendMessageCommand = new RelayCommand(SendMessage, (b) => true);

            Network.OnSuccessfullConnect += this.Network_OnSuccessfullConnect;
            Network.OnDisconnect += this.Network_OnDisconnect;
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
            Network.OnReceiveIncorrectLogin += this.Network_OnReceiveIncorrectLogin;
            Network.OnReceiveRefreshScore += this.Network_OnReceiveRefreshScore;
            Network.OnReceiveMobDeath += this.Network_OnReceiveMobDeath;
            Network.OnReceiveSingleAttack += this.Network_OnReceiveSingleAttack;
            Network.OnReceiveWhisperMessage += this.Network_OnReceiveWhisperMessage;
            Network.OnReceiveGameMessageUnknow += (a, packet) =>
            {
                Console.WriteLine($"Pacote 0x106 recebido... Type: { packet.Type.ToString("X") }. Código: { packet.Code.ToString("X") } ");
            };

            Network.OnReceiveCharLogoutSignal += (sender, args) => State = TPlayerState.Token;

            Network.Connect();
        }

        #endregion

        #region Timer

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (State != TPlayerState.Play)
                return;

            // Macro físico e mágico desabilitados
            if (!IsPhysical && !IsMagical)
                return;

            Macro.DoMacro();
        }

        #endregion
        #region Commands 

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
            //Client.SingleAttack(Player.ClientId, Player.Position, Player.Position, new WYD2.Common.OutgoingPacketStructure.MTarget(Player.ClientId, -1), 64);
            Messages.Add(new TMessage($"Movimentado de {Player.Position } para { pos } ", TMessage.NormalColor));
            Position = pos;
        }

        private void SendMessage(object obj)
        {
            string message = Message;
            if (string.IsNullOrWhiteSpace(message))
                return;

            if(message[0] == '/')
            {// whisper
                int indexOf = message.IndexOf(' ');

                string command = message.Substring(1, indexOf - 1);
                string onlyMessage = message.Substring(indexOf + 1, message.Length - indexOf - 1);

                Client.SendCommand(Player.ClientId, command, onlyMessage);
                Messages.Add(new TMessage(TMessage.NormalChat(Player.Mob.Name, onlyMessage), TMessage.WhisperColor));
            }
            else
            {
                Client.SendMessage(Player.ClientId, message);

                Messages.Add(new TMessage(TMessage.NormalChat(Player.Mob.Name, message), TMessage.NormalColor));
            }

            Message = string.Empty;
        }

        #endregion

        #region Events from Server 

        private void Network_OnDisconnect(object sender, EventArgs e)
        {
        }

        private void Network_OnReceiveWhisperMessage(object sender, MWhisperMessagePacket e)
        {
            Messages.Add(new TMessage(TMessage.WhisperChat(e.Command, e.Message), TMessage.WhisperColor));
        }

        private void Network_OnReceiveMobDeath(object sender, MMobDeathPacket e)
        {
            if (e.Killed == Player.ClientId)
            {
                _synchronizationContext.Send(async (a) =>
                {
                    await DialogHost.Show(new GameMessage("Você morreu"), "RootDialog");
                }, e);
            }
            else
            {
                var mob = Mobs.ById(e.Killed);
                if (mob == null)
                    return;

                Mobs.Remove(mob);
            }
        }

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

                if (e)
                {
                    _synchronizationContext.Send(async (a) =>
                    {
                        CharListViewModel context = new CharListViewModel(Client, SelChar);
                        CharListWindow window = new CharListWindow()
                        {
                            DataContext = context
                        };

                        await DialogHost.Show(window, "RootDialog");
                    }, null);
                }
            }, e);

            if (e)
                Player.State = TPlayerState.Token;
        }

        private void Network_OnReceiveUnknowPacket(object sender, ushort e)
        {
            Console.WriteLine($"Pacote { e.ToString("X") } desconhecido");
        }

        private void Network_OnSuccessfullConnect(object sender, EventArgs e)
        {
            Client.Login("brotheragem", "bunda123", 762);
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
            _synchronizationContext.Send((a) =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            }, null);

            Mob = e.Mob;
            Player.ClientId = e.ClientIndex;
            Position = e.Position;

            State = TPlayerState.Play;
        }

        private void Network_OnReceiveCreateMob(object sender, MCreateMobPacket e)
        {
            if (e.Index == Player.ClientId)
            {
                Position = e.Position;

                for (int i = 0; i < GameBasics.MAXL_AFFECT; i++)
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
                Mobs.Add(new MMob(e.Name, e.Index)
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
                var mob = Mobs.ById(e.Header.ClientId);
                if (mob == null)
                    return;

                Messages.Add(new TMessage(TMessage.NormalChat(mob.Name, e.Message), TMessage.NormalColor));
            }, e);
        }

        private void Network_OnReceiveMovement(object sender, MMovePacket e)
        {
            int index = e.Header.ClientId;
            if (index == Player.ClientId)
            {
                Position = e.Destiny;

                return;
            }

            var mob = Mobs.ById(index);
            if (mob == null)
                return;

            mob.Position = e.Destiny;
        }

        private void Network_OnReceiveIncorrectLogin(object sender, MIncorrectLoginPacket e)
        {
            string message = string.Empty;
            switch (e.Code)
            {
                case 131:
                    message = "Esta conta não existe";
                    break;
                case 133:
                    message = "Senha digita inválida";
                    break;
                case 161:
                    message = "Conta em análise";
                    break;
            }

            if (message == string.Empty)
                return;
            _synchronizationContext.Send(async (a) =>
            {
                await DialogHost.Show(new GameMessage(message), "RootDialog");
            }, e);
        }

        private void Network_OnReceiveRefreshScore(object sender, MRefreshScorePacket e)
        {
            int index = e.Header.ClientId;
            if (index == Player.ClientId)
            {
                FinalScore = e.Score;
            }
            else
            {
                var mob = Mobs.ById(index);

                // todo : enviar pacote de solicitar mob
                if (mob == null)
                    return;

                mob.Score = e.Score;
            }
        }

        private void Network_OnReceiveSingleAttack(object sender, MSingleAttackPacket e)
        {
            int damage = e.Target.Damage;
            int targetId = e.Target.Index;

            if (targetId == Player.ClientId)
            {
                if (damage == -1)
                    return;

                CurrentHp -= damage; 
            }
            else
            {
                var mob = Mobs.ById(targetId);
                if (mob == null)
                    return;

                //mob.Score.CurrHp -= damage;
            }
        }

        #endregion
    }
}
