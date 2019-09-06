using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYD2.Common.GameStructure
{
    public class MSpellData
    {
        public string Name;
        public int Points;
        public int Target;
        public int Mana;
        public int Delay;
        public int Range;
        public int InstanceType; // Affect[0].Index
        public int InstanceValue; // Affect[0].Value
        public int TickType; // Affect[1].Index
        public int TickValue; // Affect[1].Value
        public int AffectType; // Affect[2].Index
        public int AffectValue; // Affect[2].Value
        public int Time;
        public char[] Act;
        public int InstanceAttribute;
        public int TickAttribute;
        public int Aggressive;
        public int Maxtarget;
        public int PartyCheck;
        public int AffectResist;
        public int Passive_Check;
        public int ForceDamage;
    }
}
