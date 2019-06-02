using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;
using WYD2.Common.OutgoingPacketStructure;

namespace WYD2.Common.CommonPackets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public class MTarget
    {
        public int Index;
        public int Damage;

        public MTarget(int index, int damage)
        {
            Index = index;
            Damage = damage;
        }
    }

}
