using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;
using WYD2.Common.OutgoingPacketStructure;
using WYD2.Common.Utility;

namespace WYD2.Control.System
{
    public class PhysicalMacro : MacroSystem
    {
        public PhysicalMacro(MPlayer mob, IList<MMob> vision)
            : base(mob, vision)
        {
        }

        public override short GetNextSkill()
        {
            // físico por enquanto retorna apenas 151 por estar de arco
            return -1;
        }

        public override void DoMacro()
        {
            if (IsCurrentValid() && DoAttack())
                return;

            ClearEnemyList();

            var vision = Vision.ToList() ;
            foreach(var enemy in vision)
            {
                if (enemy.Index < 1000)
                    continue;

                double distance = W2Helper.GetDistance(enemy.Position, Player.Position);
                if (distance > Player.Range)
                    continue;

                AddEnemyList(enemy.Index, (ushort)distance);
                    
            }

            if (SelectEnemy())
                DoAttack();
        }
    }
}
