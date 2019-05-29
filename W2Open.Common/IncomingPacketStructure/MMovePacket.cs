using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Common.IncomingPacketStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MMovePacket : IGamePacket
    {
        public const ushort Opcode = 0x36C;

        public MPacketHeader Header { get; set; }

        public MPosition LastPosition;

        public UInt32 MoveType;
        public UInt32 SpeedMove;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public String Command;

        public MPosition Destiny;
    }
}
