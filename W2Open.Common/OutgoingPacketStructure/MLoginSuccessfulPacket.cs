using System;
using System.Runtime.InteropServices;
using WYD2.Common.GameStructure;

namespace WYD2.Common.OutgoingPacketStructure
{
    /// <summary>
    /// Packet sent by game client requesting login.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MLoginSuccessfulPacket : IGamePacket
    {
        public const ushort Opcode = 0x10A;

        public MPacketHeader Header { get; set; }

        public unsafe fixed sbyte HashKeyTable[16];
        
        public int Offset_28; // TODO: unknown!

        public MSelChar SelChar;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_CARGO_ITEM)]
        public MItem[] Cargo;

        public int CargoCoin;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MLoginInfo.MAXL_ACCNAME)]
        public String AccName;

        public unsafe fixed sbyte Keys[12];
    }
}