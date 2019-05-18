using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var packet = MAccountLoginPacket.Create(userName, password, version);

            Connection.Send(W2Marshal.GetBytes(packet));
        }

        public void SendToken(string password, int isChanging)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password cannot be null");

            Connection.Send(W2Marshal.GetBytes(MTokenPacket.Create(password, isChanging)));
        }

        public void EnterMob(int charIndex)
        {
            Connection.Send(W2Marshal.GetBytes(MRequestMobLogin.Create(charIndex)));
        }

        public void CreateCharacter(string name, int slotId, int classId)
        {
            Connection.Send(W2Marshal.GetBytes(MCreateCharacterPacket.Create(name, slotId, classId)));
        }
    }
}
