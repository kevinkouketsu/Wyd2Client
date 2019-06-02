using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Common.IncomingPacketStructure
{
    public struct MRefreshScorePacket : IGamePacket
    {
        public const ushort Opcode = 0x336;

        public MPacketHeader Header { get; set; }

        public MScore Score;

        public byte Critical;
        public byte SaveMana;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameBasics.MAXL_AFFECT)]
        public MAffectPacket[] Affect;

        public ushort GuildIndex;

        public byte RegenHP;
        public byte RegenMP;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Resist;

        public ushort Unknow_01;

        public ushort CurrentHp;
        public ushort CurrentMp;

        public byte Unknow_02;
        public byte MagicIncrement;

        public int Unknow_03;
    }
}
