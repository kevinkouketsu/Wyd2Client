using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.Utility;

namespace WYD2.Common.GameStructure
{
    public enum TPlayerState
    {
        Empty = 0,
        Hello = 1,
        SelChar = 11,
        Token = 12,
        Play = 22
    }

    public class MPlayer
    {
        public MSelChar SelChar { get; set; }

        public ushort ClientId { get; set; }
        public TPlayerState State { get; set; }

        private MMobCore _mob;
        public MMobCore Mob
        {
            get => _mob;
            set
            {
                _mob = value;

                Range = W2Objects.GetMobAbility(value, MItemDefinition.RANGE);
            }
        }
        public MPosition Position { get; set; }

        public int Range { get; set; } = 3;

        public MAffect[] Affects { get; set; } = new MAffect[GameBasics.MAXL_AFFECT];

        public string SelectedCharlistCharacter { get; set; }

        public MPlayer()
        {
            Mob = new MMobCore();
        }
    }
}
