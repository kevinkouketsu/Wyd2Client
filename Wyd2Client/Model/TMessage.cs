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
    public struct TMessage
    {
        public string Message { get; }
        public SolidColorBrush Color { get; }

        public TMessage(string message, SolidColorBrush color)
        {
            Color = color;
            Message = message;
        }

        public static string WhisperChat(string nickname, string message)
        {
            return $"[{ nickname }]> { message }";
        }

        public static string NormalChat(string nickname, string message)
        {
            return $"[{nickname}]> { message }";
        }

        public static SolidColorBrush WhisperColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF00"));
        public static SolidColorBrush NormalColor = new SolidColorBrush(Colors.White);
        public static SolidColorBrush SystemColor = new SolidColorBrush(Colors.BlueViolet);
    }
}
