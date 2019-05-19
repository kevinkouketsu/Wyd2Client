﻿using MaterialDesignThemes.Wpf;
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
using WYD2.Common.GameStructure;
using WYD2.Common.IncomingPacketStructure;
using WYD2.Network;

namespace Wyd2.Client.ViewModel
{
    public class PlayerViewModel : BaseViewModel
    {
        private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        private ClientConnection Network { get; }
        private ClientControl Client { get; set; }

        private MainWindowModel Player { get; }
        private MSelChar _selChar;

        private int SelectedCharacterIndex
        {
            get => SelChar.Names.IndexOf(SelChar.Names.First(x => x.Name == SelectedCharlistCharacter.Name));
        }

        public bool IsSelCharExpanded
        {
            get => Player.IsSelCharExpanded;
            set
            {
                Player.IsSelCharExpanded = value;

                OnPropertyChanged();
            }
        }

        public MMobName SelectedCharlistCharacter
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
            get => _selChar;
            set
            {
                _selChar = value;

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

        public ICommand CreateCharacterCommand { get; }
        public ICommand DeleteCharacterCommand { get; }
        public ICommand EnterCharacterCommand { get; }

        public PlayerViewModel()
        {
            Player = new MainWindowModel();

            Network = new ClientConnection("192.168.15.12", 8281);

            Console.WriteLine(Marshal.SizeOf(typeof(MCharToWorldPacket)));

            // Commands 
            CreateCharacterCommand = new RelayCommand(CreateCharacter, CanCreateCharacter);
            DeleteCharacterCommand = new RelayCommand(DeleteCharacter, CanDeleteCharacter);
            EnterCharacterCommand = new RelayCommand(EnterCharacter, CanEnterCharacter);

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
            Network.OnReceiveCharLogoutSignal += (sender, args) => State = TPlayerState.Token;

            Network.Connect();
        }

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

            if (State == TPlayerState.Token && !string.IsNullOrWhiteSpace(SelectedCharlistCharacter.Name))
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

                Client.DeleteCharacter(SelectedCharlistCharacter.Value, index, password);
            }
        }

        private bool CanDeleteCharacter()
        {
            return Player.State == TPlayerState.Token && !string.IsNullOrWhiteSpace(SelectedCharlistCharacter.Value);
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
            Messages.Add("A tela de seleção de personagens foi atualizada");

            IsSelCharExpanded = true;
        }

        private void Network_OnTokenResponse(object sender, bool e)
        {
            Messages.Add(e ? "Senha numérica correta" : "Senha numérica incorreta");

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
            Client.Login("zeus", "zeus", 1001);
        }

        private void Network_OnSucessfullLogin(object sender, MLoginSuccessfulPacket e)
        {
            SelChar = e.SelChar;
            AccountName = e.AccName;

            Client.SendToken("1234", 0);

            Player.State = TPlayerState.SelChar;
            IsSelCharExpanded = true;
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
            Player.Mob = e.Mob;

            State = TPlayerState.Play;
            IsSelCharExpanded = false;
        }

        private void Network_OnReceiveCreateMob(object sender, MCreateMobPacket e)
        {
            Console.WriteLine($"{ e.Name } apareceu na tela em { e.Position.X }x { e.Position.Y }y. TAB { e.Tab }");
        }

        #endregion

        #region Public Properties

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

        public ObservableCollection<ushort> UnknowPackets { get; set; } = new AsyncObservableCollection<ushort>();
        public ObservableCollection<string> Messages { get; set; } = new AsyncObservableCollection<string>();

        #endregion
    }
}
