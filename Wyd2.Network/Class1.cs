using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common;

namespace WYD2.Network
{
    public class TCP_StateConnection
    {
        public byte[] packetBuffer;
        public byte[] recvBuffer = new byte[NetworkBasics.MAXL_PACKET];

        public Socket workSocket;
        public int RecvPos;
        public int PacketSize;
    }

    public class NetworkClient
    {
        public IPAddress IpAddress { get; }
        public int Port { get; }

        public TCP_StateConnection state { get; private set; }

        public NetworkClient(string ipAddress, int port)
        {
            if (!IPAddress.TryParse(ipAddress, out IPAddress ip))
                throw new ArgumentException($"O argumento { ipAddress } { nameof(ipAddress) } é inválido");

            IpAddress = ip;
            Port = port;
        }

        private void Connect()
        {
            IPEndPoint remoteEP = new IPEndPoint(IpAddress, Port);

            // Create a TCP/IP socket.  
            Socket client = new Socket(IpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }
    }
}
