using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.CommonPackets;
using WYD2.Common.GameStructure;
using WYD2.Common.OutgoingPacketStructure;
using WYD2.Common.Utility;

namespace WYD2.Control
{
    public class ClientControl
    {
        private ClientConnection Connection { get; }

        private ushort _attackCount;
        public ushort AttackCount
        {
            get
            {
                ushort attackCount = _attackCount++;

                if (_attackCount >= ushort.MaxValue)
                    _attackCount = 0;

                return attackCount;
            }
        }

        public ClientControl(ClientConnection connection)
        {
            Connection = connection;
        }

        public void Login(string userName, string password, int version)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("Username cannot be null");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password cannot be null");

            var packet = new MAccountLoginPacket(userName, password, version);
            Connection.Send(W2Marshal.GetBytes(packet));
        }

        public void SendToken(string password, int isChanging)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password cannot be null");

            Connection.Send(W2Marshal.GetBytes(new MTokenPacket(password, isChanging)));
        }

        public void EnterMob(int charIndex)
        {
            Connection.Send(W2Marshal.GetBytes(new MRequestMobLogin(charIndex)));
        }

        public void CreateCharacter(string name, int slotId, int classId)
        {
            Connection.Send(W2Marshal.GetBytes(new MCreateCharacterPacket(name, slotId, classId)));
        }

        public void DeleteCharacter(string name, int slotId, string password)
        {
            Connection.Send(W2Marshal.GetBytes(new MDeleteCharacterPacket(slotId, name, password)));
        }

        public void CharLogout()
        {
            Connection.Send(W2Marshal.GetBytes(new MPacketSignal(0x215, 0)));
        }

        public void Movement(ushort clientId, MPosition Current, MPosition Destiny, uint moveType, uint Speed)
        {
            var movement = new MMovePacket(clientId, Current, Destiny, moveType, Speed);

            Connection.Send(W2Marshal.GetBytes(movement));
        }

        public void SingleAttack(ushort clientId, MPosition attackerPosition, MPosition targetPosition, MTarget target, short skilId)
        {
            var packet = new MSingleAttackPacket(clientId)
            {
                AttackCount = AttackCount,
                AttackerPosition = attackerPosition,
                AttackerId = clientId,

                Target = target,
                TargetPosition = targetPosition,

                CurrentMp = -1,
                SkillId = skilId,
                FlagLocal = 1,
                Motion = 4,
            };

            Connection.Send(W2Marshal.GetBytes(packet));
        }

        public void SendCommand(ushort clientId, string command, string message)
        {
            var packet = new MWhisperMessagePacket(clientId);
            packet.Command = command;
            packet.Message = message;

            Connection.Send(W2Marshal.GetBytes(packet));
        }

        public void SendMessage(ushort clientId, string message)
        {
            Connection.Send(W2Marshal.GetBytes(new MChatMessagePacket(clientId, message)));
        }

        public void Reborn(ushort clientId)
        {
            Connection.Send(W2Marshal.GetBytes(new MPacketSignal(0x291, clientId)));
        }

        public void UseTeleport(ushort clientId)
        {
            Connection.Send(W2Marshal.GetBytes(new MPacketSignal(0x290, clientId)));
        }

        public void SendPacket<T>(ClientPacket<T> packet)
        {
            Connection.Send(W2Marshal.GetBytes(packet));
        }
    }
}
