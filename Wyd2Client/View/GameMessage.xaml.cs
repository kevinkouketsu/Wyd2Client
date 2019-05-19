using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wyd2.Client.ViewModel;

namespace Wyd2.Client.View
{
    /// <summary>
    /// Interaction logic for GameMessage.xaml
    /// </summary>
    public partial class GameMessage : UserControl
    {
        public GameMessage(string message)
        {
            InitializeComponent();

            DataContext = new GameMessageViewModel()
            {
                Message = message
            };
        }
    }
}
