using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common;
using WYD2.Common.CommonPackets;
using WYD2.Common.GameStructure;
using WYD2.Common.OutgoingPacketStructure;
using WYD2.Common.Utility;

namespace WYD2.Control.System
{
    public abstract class MacroSystem : IMacro
    {
        public struct Enemy
        {
            public ushort Id { get; set; }
            public ushort Distance { get; set; }

            public Enemy(ushort id, ushort distance)
            {
                Id = id;
                Distance = distance;
            }

            public void ChangeDistance(ushort distance)
            {
                Distance = distance;
            }
        }

        public event EventHandler<MSingleAttackPacket> OnAttackMob;

        protected IList<MMob> Vision { get; }
        protected MPlayer Player { get; }

        public MacroSystem(MPlayer mob, IList<MMob> vision)
        {
            Vision = vision;
            Player = mob;
        }

        public int CurrentEnemy { get; private set; }

        protected IList<Enemy> Enemies { get; } = new List<Enemy>();

        public abstract short GetNextSkill();
        public abstract void DoMacro();

        public bool IsCurrentValid()
        {
            var mobs = Vision.Where(x => x.Index == CurrentEnemy);
            if (mobs.Count() <= 0)
                return false;

            var mob = mobs.First();
            if (W2Helper.GetDistance(mob.Position, Player.Position) > Player.Range)
                return false;

            if (mob.Score.CurrHp <= 0 || mob.Index < 1000)
                return false;

            return true;
        }

        protected bool AddEnemyList(ushort enemyId, ushort distance)
        {
            if (enemyId <= 1000)
                return false;

            foreach(var enemy in Enemies)
            {
                if(enemy.Id == enemyId)
                {
                    enemy.ChangeDistance(distance);

                    return true;
                }
            }

            if (Enemies.Count >= ProjectBasics.MAX_ENEMIES)
                return false;

            Enemies.Add(new Enemy(enemyId, distance));
            return true;
        }

        protected void ClearEnemyList()
        {
            Enemies.Clear();
        }

        protected bool SelectEnemy()
        {
            int distance = 100;
            int enemyId = 0;

            foreach(var enemy in Enemies)
            {
                if(enemy.Distance < distance)
                {
                    distance = enemy.Distance;
                    enemyId = enemy.Id;
                }
            }

            if (distance == 100 || enemyId == 0 || enemyId >= GameBasics.MAX_SPAWN_MOB)
            {
                CurrentEnemy = 0;
                return false;
            }

            CurrentEnemy = enemyId;
            return true;
        }

        protected bool DoAttack()
        {
            MMob mob;
            try
            {
                mob = Vision.First(x => x.Index == CurrentEnemy);
            }
            catch
            {
                return false;
            }
            
            var packet = new MSingleAttackPacket(Player.ClientId);
            packet.Target = new MTarget(CurrentEnemy, -2);

            packet.AttackerId = Player.ClientId;
            packet.AttackerPosition = Player.Position;
            packet.TargetPosition = mob.Position;

            packet.SkillId = GetNextSkill();
            packet.FlagLocal = 1;
            packet.Motion = 4;
            packet.CurrentMp = -1;

            OnAttackMob?.Invoke(this, packet);
            return true;
        }
    }
}
