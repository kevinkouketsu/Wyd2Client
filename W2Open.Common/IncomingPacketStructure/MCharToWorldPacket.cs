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
    public struct MCharToWorldPacket : IGamePacket
    {
        public const int Opcode = 0x114;

        public MPacketHeader Header { get; set; }

        public MPosition Position;

        public MMobCore Mob;
        public MScore Score;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 26)]
        public string Tab;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string GuildName;

        public int NextMovement;

        public unsafe fixed byte Unknow_1[108];

        public short SlotIndex;
        public short ClientIndex;
        public short Evasion;

        public unsafe fixed byte Skillbar2[16];

        public unsafe fixed byte Unknow_3[42];

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_AFFECT)]
        public MAffect[] Affect;

        public int ClassMaster;

        public long Unknow_4;

        public unsafe fixed byte Unknow_5[30];
        public unsafe fixed byte Unknow_6[20];
        public unsafe fixed byte Unknow_7[278];
    }
}
