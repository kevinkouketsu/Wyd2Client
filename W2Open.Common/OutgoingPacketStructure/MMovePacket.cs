using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Common.OutgoingPacketStructure
{
    [StructLayout(LayoutKind.Sequential, Pack = ProjectBasics.DEFAULT_PACK, CharSet = CharSet.Ansi)]
    public class MMovePacket : ClientPacket<MMovePacket>
    {
        public const ushort opcode = 0x36C;

        public MPosition LastPosition;

        public UInt32 MoveType;
        public UInt32 SpeedMove;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public String Command;

        public MPosition Destiny;

        public MMovePacket(ushort clientId, MPosition lastPosition, MPosition destPosition, UInt32 moveType, UInt32 speedMove)
            : base(opcode, clientId)
        {
            LastPosition = lastPosition;

            MoveType = moveType;
            SpeedMove = speedMove;

            Destiny = destPosition;
        }
    }
}
