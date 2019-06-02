using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Common.IncomingPacketStructure
{
    public struct MIncorrectLoginPacket : IGamePacket
    {
        public const ushort Opcode = 0x105;

        public MPacketHeader Header { get; set; }

        public ushort Unknow;
        public ushort Code;
    }
}
