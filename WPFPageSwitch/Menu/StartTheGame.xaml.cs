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
    /// Interaction logic for StartTheGame.xaml
    /// </summary>
    public partial class StartTheGame : UserControl , ISwitchable
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        public StartTheGame()
        {
            InitializeComponent();
        }
        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();
            button.Visibility = Visibility.Hidden;


        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // code goes here
            progressBar.Value+=5;
            if (progressBar.Value == 100)
            {
                dispatcherTimer.Stop();
                Switcher.Switch(new CreateGameAndJoin());
            }
        }

        
        #endregion
    }
}
