using System;
using System.Runtime.InteropServices;
using WYD2.Common.GameStructure;
using WYD2.Common.Utility;

namespace WYD2.Common.OutgoingPacketStructure
{
    /// <summary>
    /// Packet sent by game client requesting login.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public class MAccountLoginPacket : ClientPacket<MAccountLoginPacket>
    {
        public const ushort Opcode = 0x20D;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public String Password;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public String AccName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 52)]
        public Byte[] Zero;

        public int ClientVersion;
        public int DBNeedSave;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] AdapterName;

        public MAccountLoginPacket(string accountId, string password, int clientVersion)
            : base(Opcode, 0)
        {
            AccName = accountId;
            ClientVersion = clientVersion;
            Password = password;

            DBNeedSave = 1;
        }
    }
}