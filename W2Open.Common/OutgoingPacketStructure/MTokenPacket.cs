using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;
using WYD2.Common.Utility;

namespace WYD2.Common.OutgoingPacketStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public class MTokenPacket : ClientPacket<MTokenPacket>
    {
        public const ushort Opcode_Incorrect = 0xFDF;
        public const ushort Opcode = 0xFDE;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string Password;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] Unknown;

        public int IsChanging;

        public MTokenPacket(string password, int isChanging)
            : base(Opcode, 0)
        {
            Password = password;
            IsChanging = isChanging;
        }
    }
}
