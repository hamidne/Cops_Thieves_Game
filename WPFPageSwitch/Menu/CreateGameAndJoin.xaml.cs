using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFPageSwitch.Menu
{
    /// <summary>
    /// Interaction logic for CreateGameAndJoin.xaml
    /// </summary>
    public partial class CreateGameAndJoin : UserControl, ISwitchable
    {
        public CreateGameAndJoin()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void CreateGameBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new SetGameInformation());
        }

        private void JoinGameBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new JoinTheGame());
        }


    }
}
