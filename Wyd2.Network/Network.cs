using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WYD2.Common;
using WYD2.Common.GameStructure;
using WYD2.Common.Utility;

namespace WYD2.Control
{
    public class TCP_StateConnection
    {
        public byte[] packetBuffer;
        public byte[] recvBuffer = new byte[NetworkBasics.MAX_PACKET_LENGTH];

        public Socket workSocket;
        public int RecvPos;
        public int PacketSize;
    }

    public abstract class TcpBase
    {
        public object _locker = new object();
        protected event EventHandler<EventArgs> OnSuccessfullConnect;
        protected event EventHandler<EventArgs> OnDisconnect;

        protected abstract void InterpretPacket(int packetId, byte[] buffer);

        private Thread _recvThread { get; set; }
        private bool InitCode { get; set; }

        public IPAddress IpAddress { get; }
        public int Port { get; }

        private readonly ManualResetEvent InitCodeEvent = new ManualResetEvent(false);

        protected TCP_StateConnection State { get; private set; }

        public TcpBase(string ipAddress, int port)
        {
            if (!IPAddress.TryParse(ipAddress, out IPAddress ip))
                throw new ArgumentException($"O argumento { ipAddress } { nameof(ipAddress) } é inválido");

            IpAddress = ip;
            Port = port;
        }

        public void Connect()
        {
            IPEndPoint remoteEP = new IPEndPoint(IpAddress, Port);

            // Create a TCP/IP socket.  
            Socket client = new Socket(IpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
        }

        public void Send(byte[] data)
        {
            if (data.Length >= 12)
                PacketSecurity.Encrypt(data);

            // Begin sending the data to the remote device.  
            State.workSocket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), State);
        }

        private void SendCallback(IAsyncResult ar)
        {
            if (!InitCode)
            {
                InitCodeEvent.Set();

                InitCode = true;

                OnSuccessfullConnect?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);

            // Create the state object.  
            State = new TCP_StateConnection
            {
                workSocket = client
            };

            // Envia o pacote de HelloWorld e espera até 3 segundos para a resposta
            Send(BitConverter.GetBytes(NetworkBasics.INIT_CODE));

            InitCodeEvent.WaitOne(3000);

            StartReceive(State);
        }

        private void StartReceive(TCP_StateConnection state)
        {
            state.workSocket.BeginReceive(state.recvBuffer, 0, NetworkBasics.MAX_PACKET_LENGTH, 0, new AsyncCallback(ReadCallback), state);
        }

        private unsafe void ReadCallback(IAsyncResult ar)
        {
            State = (TCP_StateConnection)ar.AsyncState;

            try
            {
                Socket handler = State.workSocket;
                int read = handler.EndReceive(ar);

                var packetBuffer = new CCompoundBuffer(State.recvBuffer);

                if (read == 0)
                {
                    OnDisconnect?.Invoke(this, EventArgs.Empty);
                    return;
                }

                lock (_locker)
                {
                    while (packetBuffer.Offset < read)
                    {
                        PacketSecurity.Decrypt(packetBuffer);

                        int size = packetBuffer.ReadNextShort(0);
                        int packetId = packetBuffer.ReadNextShort(4);

                        if (size < 12)
                            break;

                        byte[] buffer = new byte[size];
                        fixed (byte* pBuffer = &packetBuffer.RawBuffer[packetBuffer.Offset])
                        {
                            Marshal.Copy((IntPtr)pBuffer, buffer, 0, size);
                        }

                        InterpretPacket(packetId, buffer);
                        packetBuffer.Offset += size;
                    }
                }

                StartReceive(State);
            }
            catch(SocketException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
