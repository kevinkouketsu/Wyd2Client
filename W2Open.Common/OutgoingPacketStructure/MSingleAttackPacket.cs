using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.CommonPackets;
using WYD2.Common.GameStructure;

namespace WYD2.Common.OutgoingPacketStructure
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = ProjectBasics.DEFAULT_PACK)]
    public class MSingleAttackPacket : ClientPacket<MSingleAttackPacket>
    {
        public const ushort Opcode = 0x39D;

        public int Hold;
        public int ReqMp;
        public int Unknow_01;
        public long CurrentExp;
        public ushort Unknow_02;

        public MPosition AttackerPosition;
        public MPosition TargetPosition;

        public ushort AttackerId;
        public ushort AttackCount;

        public byte Motion;
        public byte SkillParm;
        public byte DoubleCritical;
        public byte FlagLocal;

        public ushort Rsv;

        public int CurrentMp;
        public short SkillId;
        public short Unknow_3;

        public MTarget Target;

        public MSingleAttackPacket(ushort clientId)
            : base(0x39D, clientId)
        {
        }
    }
}
