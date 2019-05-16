using System;
using System.Runtime.InteropServices;
using WYD2.Common.GameStructure;
using WYD2.Common.Utility;

namespace WYD2.Common.OutgoingPacketStructure
{
    /// <summary>
    /// A text message which will be displayed in the client as a game notice.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MTextMessagePacket : IGamePacket
    {
        public const ushort Opcode = 0x101;

        public MPacketHeader Header { get; set; }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 96)]
        public String Message;

        public static MTextMessagePacket Create(String msg)
        {
            MTextMessagePacket packet = W2Marshal.GetEmptyValid<MTextMessagePacket>(Opcode);
            packet.Message = msg;
            return packet;
        }
    }
}