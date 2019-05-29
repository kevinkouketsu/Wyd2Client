using System.Runtime.InteropServices;
using WYD2.Common.Utility;

namespace WYD2.Common.GameStructure
{
    /// <summary>
    /// All the packet structures must implement this interface.
    /// </summary>
    public interface IGamePacket
    {
        MPacketHeader Header { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public abstract class ClientPacket<T>
    {
        public MPacketHeader Header;

        public ClientPacket(ushort packetId, ushort clientId)
        {
            W2Marshal.BuildPacketHeader<T>(ref Header, clientId);

            Header.Opcode = packetId;
        }
    }

    /// <summary>
    /// Header present in all the valid game packets.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK)]
    public struct MPacketHeader
    {
        public ushort Size;

        public byte Key;
        public byte CheckSum;

        public ushort Opcode;
        public ushort ClientId;

        public int TimeStamp;
    }
}