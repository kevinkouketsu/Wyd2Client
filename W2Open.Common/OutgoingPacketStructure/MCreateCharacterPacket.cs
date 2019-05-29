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
    public class MCreateCharacterPacket : ClientPacket<MCreateCharacterPacket>
    {
        public const int Opcode = 0x20F;

        public int SlotId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Name;

        public int ClassId;

        public MCreateCharacterPacket(string name, int slotId, int classId)
            : base(Opcode, 0)
        {
            ClassId = classId;
            Name = name;
            SlotId = slotId;
        }
    }
}
