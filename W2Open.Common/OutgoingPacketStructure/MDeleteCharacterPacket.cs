using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Common.OutgoingPacketStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public class MDeleteCharacterPacket : ClientPacket<MDeleteCharacterPacket>
    {
        public const int Opcode = 0x211;

        public int SlotIndex;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string Password;

        public MDeleteCharacterPacket(int slotId, string name, string password)
            : base (Opcode, 0)
        {
            Name = name;
            Password = password; SlotIndex = slotId;
        }
    }
}
