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
    public struct MAffectCreateMobPacket
    {
        public byte Index;
        public byte Time;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MCreateMobPacket : IGamePacket
    {
        public const ushort Opcode = 0x364;

        public MPacketHeader Header { get; set; }

        public MPosition Position { get; set; }

        public short Index;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string Name;

        public byte ChaosPoint;
        public byte CurrentKill;
        public byte TotalKill;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public ushort[] Items;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_AFFECT)]
        public MAffectCreateMobPacket[] Affect;

        public ushort GuildIndex;

        public byte GuildMemberType;
        public short Unknow;

        public MScore Score;

        public short Type;

        public unsafe fixed byte AnctCode[16];

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 26)]
        public string Tab;

        public unsafe fixed byte Unknow_02[4];
    }
}
