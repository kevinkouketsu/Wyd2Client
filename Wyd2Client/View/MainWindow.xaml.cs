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
using WYD2.Network;

namespace Wyd2.Client.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Random rand = new Random();
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new PlayerViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int posX = rand.Next(1200, 2500);
            int posY = rand.Next(1200, 2500);

            MessageBox.Show($" { posX } { posY }");
        }
    }
}
