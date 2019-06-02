    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Common.IncomingPacketStructure
{
    public struct MMobDeathPacket : IGamePacket
    {
        public const ushort Opcode = 0x338;

        public MPacketHeader Header { get; set; }

        public int Hold;
        public ushort Killed;
        public ushort Killer;

        public int Unknow;
        public long Exp;
    }
}
