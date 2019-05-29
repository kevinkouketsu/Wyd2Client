using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common;
using WYD2.Common.GameStructure;
using WYD2.Common.Utility;

namespace WYD2.Common.OutgoingPacketStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public class MRequestMobLogin : ClientPacket<MRequestMobLogin>
    {
        public const ushort Opcode = 0x213;

        public int CharIndex;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
        public byte[] Unknow;

        public MRequestMobLogin (int charIndex)
            : base(Opcode, 0)
        {
            CharIndex = charIndex;
        }
    }
}
