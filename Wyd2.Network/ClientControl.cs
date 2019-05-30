using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;
using WYD2.Common.IncomingPacketStructure;
using WYD2.Common.OutgoingPacketStructure;
using WYD2.Common.Utility;

namespace WYD2.Network
{
    public class ClientControl
    {
        private ClientConnection Connection { get; }

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
            var movement = new Common.OutgoingPacketStructure.MMovePacket(clientId, Current, Destiny, moveType, Speed);

            Connection.Send(W2Marshal.GetBytes(movement));
        }
    }
}
