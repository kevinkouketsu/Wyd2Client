using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;
using WYD2.Common.IncomingPacketStructure;
using WYD2.Common.OutgoingPacketStructure;
using WYD2.Common.Utility;
using WYD2.Network;

namespace WYD2.Network
{
    public class ClientConnection : TcpBase
    {
        public event EventHandler<MLoginSuccessfulPacket> OnSucessfullLogin;
        public event EventHandler<ushort> OnReceiveUnknowPacket;
        public event EventHandler<bool> OnTokenResponse;
        public event EventHandler<MResendCharList> OnRefreshCharList;

        public new event EventHandler<EventArgs> OnSuccessfullConnect
        {
            add { base.OnSuccessfullConnect += value; }
            remove { base.OnSuccessfullConnect -= value; }
        }

        public ClientConnection(string ipAddress, int port) 
            : base(ipAddress, port)
        {
        }

        protected override void InterpretPacket(int packetId, byte[] buffer)
        {
            Console.WriteLine($"PacketId 0x{ packetId.ToString("X") }");

            switch (packetId)
            {
                case MLoginSuccessfulPacket.Opcode:
                    OnSucessfullLogin?.Invoke(this, W2Marshal.GetStructure<MLoginSuccessfulPacket>(buffer));
                    break;
                case MTokenPacket.Opcode:
                case MTokenPacket.Opcode_Incorrect:
                    OnTokenResponse?.Invoke(this, packetId == MTokenPacket.Opcode);
                    break;
                case MResendCharList.Opcode:
                    OnRefreshCharList?.Invoke(this, W2Marshal.GetStructure<MResendCharList>(buffer));
                    break;
                default:
                    OnReceiveUnknowPacket?.Invoke(this, (ushort)packetId);
                    break;
            }
        }
    }
}
