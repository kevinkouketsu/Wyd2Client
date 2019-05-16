using System.Runtime.InteropServices;
using WYD2.Common.GameStructure;

namespace WYD2.Common.IncomingPacketStructure
{
    /// <summary>
    /// A simple ping packet sent periodically from the client to maintain the connection with the server active.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MPingPacket : IGamePacket
    {
        public const ushort Opcode = 0x3A0;

        public MPacketHeader Header { get; set; }
    }
}