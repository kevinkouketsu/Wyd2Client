using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Common.IncomingPacketStructure
{
    /// <summary>
    /// A text message which will be displayed in the client as a game notice.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MChatMessagePacket : IGamePacket
    {
        public const ushort Opcode = 0x333;

        public MPacketHeader Header { get; set; }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 96)]
        public String Message;
    }
}
