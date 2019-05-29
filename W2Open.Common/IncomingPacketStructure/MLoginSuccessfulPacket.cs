using System;
using System.Runtime.InteropServices;
using WYD2.Common.GameStructure;

namespace WYD2.Common.IncomingPacketStructure
{
    /// <summary>
    /// Packet sent by game client requesting login.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MLoginSuccessfulPacket : IGamePacket
    {
        public const ushort Opcode = 0x10A;

        public MPacketHeader Header { get; set; }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] HashKeyTable;
        
        public int Offset_28; // TODO: unknown!

        public MSelChar SelChar;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_CARGO_ITEM)]
        public MItem[] Cargo;

        public int CargoCoin;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string AccName;

        public unsafe fixed sbyte Keys[12];
    }
}