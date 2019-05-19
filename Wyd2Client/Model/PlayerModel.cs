using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace Wyd2.Client.Model
{
    public enum TPlayerState
    {
        Empty = 0,
        Hello = 1,
        SelChar = 11,
        Token = 12,
        Play = 22
    }

    public class MainWindowModel
    {
        public int ClientId { get; set; }
        public TPlayerState State { get; set; }

        public MMobCore Mob { get; set; }

        public MMobName SelectedCharlistCharacter { get; set; }

        public bool IsSelCharExpanded { get; set; }

        public MainWindowModel()
        {
            Mob = new MMobCore();
        }
    }
}
