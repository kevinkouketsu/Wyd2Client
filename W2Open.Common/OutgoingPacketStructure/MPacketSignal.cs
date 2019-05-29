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
    public class MPacketSignal : ClientPacket<MPacketSignal>
    {
        public MPacketSignal(ushort packetId, ushort clientId)
            : base(packetId, clientId)
        {

        }
    }
}
