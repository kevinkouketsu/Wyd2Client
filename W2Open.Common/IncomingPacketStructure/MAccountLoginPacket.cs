using System;
using System.Runtime.InteropServices;
using WYD2.Common.GameStructure;

namespace WYD2.Common.IncomingPacketStructure
{
    /// <summary>
    /// Packet sent by game client requesting login.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MAccountLoginPacket : IGamePacket
    {
        public const ushort Opcode = 0x20D;

        public MPacketHeader Header { get; set; }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MLoginInfo.MAXL_PSW)]
        public String Password;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MLoginInfo.MAXL_ACCNAME)]
        public String AccName;

        public unsafe fixed byte Zero[52];

        public int ClientVersion;

        public int DBNeedSave;

        public unsafe fixed int AdapterName[4];
    }
}