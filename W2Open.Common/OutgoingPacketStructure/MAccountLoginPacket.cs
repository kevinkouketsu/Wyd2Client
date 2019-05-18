using System;
using System.Runtime.InteropServices;
using WYD2.Common.GameStructure;

namespace WYD2.Common.OutgoingPacketStructure
{
    /// <summary>
    /// Packet sent by game client requesting login.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MAccountLoginPacket : IGamePacket
    {
        public const ushort Opcode = 0x20D;

        public MPacketHeader Header { get; set; }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public String Password;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public String AccName;

        // 
        public unsafe fixed byte Zero[52];

        public int ClientVersion;
        public int DBNeedSave;

        public unsafe fixed int AdapterName[4];

        public static MAccountLoginPacket Create(string accountId, string password, int clientVersion)
        {
            MAccountLoginPacket packet = new MAccountLoginPacket();
            packet.Header = new MPacketHeader
            {
                Opcode = Opcode,
                Size = (ushort)Marshal.SizeOf(typeof(MAccountLoginPacket)),
            };

            packet.AccName = accountId;
            packet.ClientVersion = clientVersion;
            packet.Password = password;

            return packet;
        }
    }
}