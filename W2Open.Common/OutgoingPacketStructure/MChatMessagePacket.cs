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
    public class MChatMessagePacket : ClientPacket<MChatMessagePacket>
    {
        public const ushort Opcode = 0x333;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 96)]
        public string Message;

        public MChatMessagePacket(ushort clientId, string message)
            : base (Opcode, clientId)
        {
            Message = message;
        }

    }
}
