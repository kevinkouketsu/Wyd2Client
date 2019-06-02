using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WYD2.Common.GameStructure;

namespace Wyd2.Client.Model
{
    public class CharListModel
    {
        public string SelectedCharlistCharacter { get; set; }

        public MSelChar SelChar { get; set; }
        public TPlayerState State { get; set; }
    }
}
