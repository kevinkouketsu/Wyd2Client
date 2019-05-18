using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common;
using WYD2.Common.GameStructure;

namespace WYD2.Common.OutgoingPacketStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MRequestMobLogin : IGamePacket
    {
        public const ushort Opcode = 0x213;
        public MPacketHeader Header { get; set; }

        public int CharIndex;

        public unsafe fixed sbyte Unknow[18];

        public static MRequestMobLogin Create(int charIndex) => new MRequestMobLogin()
        {
            CharIndex = charIndex,
            Header = new MPacketHeader()
            {
                Opcode = Opcode,
                Size = (ushort)Marshal.SizeOf(typeof(MRequestMobLogin))
            },
        };
    }
}
