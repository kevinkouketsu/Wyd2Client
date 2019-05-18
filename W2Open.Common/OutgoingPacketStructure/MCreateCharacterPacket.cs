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
    public struct MCreateCharacterPacket : IGamePacket
    {
        public const int Opcode = 0x20F;

        public MPacketHeader Header { get; set; }

        public int SlotId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Name;

        public int ClassId;

        public static MCreateCharacterPacket Create(string name, int slotId, int classId) =>
           new MCreateCharacterPacket()
           {
               Header = new MPacketHeader()
               {
                   Opcode = Opcode,
                   Size = (ushort)Marshal.SizeOf(typeof(MCreateCharacterPacket))
               },
               ClassId = classId,
               Name = name,
               SlotId = slotId
           };
    }
}
