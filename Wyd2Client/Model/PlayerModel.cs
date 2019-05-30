using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WYD2.Common;
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

    public struct TMessage
    {
        public string Message { get; }
        public SolidColorBrush Color { get; }

        public TMessage(string message, SolidColorBrush color)
        {
            Color = color;
            Message = message;
        }

        public static SolidColorBrush NormalColor = new SolidColorBrush(Colors.White);
        public static SolidColorBrush SystemColor = new SolidColorBrush(Colors.BlueViolet);
    }

    public class MainWindowModel
    {
        public MSelChar SelChar { get; set; }

        public ushort ClientId { get; set; }
        public TPlayerState State { get; set; }

        public MMobCore Mob { get; set; }
        public MPosition Position { get; set; }

        public MAffect[] Affects { get; set; } = new MAffect[GameBasics.MAXL_AFFECT];

        public string SelectedCharlistCharacter { get; set; }

        public bool IsSelCharExpanded { get; set; }

        public MainWindowModel()
        {
            Mob = new MMobCore();
        }
    }
}
