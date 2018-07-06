using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for GamePlay.xaml
    /// </summary>
    public partial class GamePlay : UserControl
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public GamePlay()
        {
            InitializeComponent();
            label.Content = "Waiting for other players to join";
            worker.DoWork += WaitForResponse;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
            
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void WaitForResponse(object sender, DoWorkEventArgs e)
        {
            while (SocketClient.response == "") ;
            Match match = Regex.Match(SocketClient.response, @"^allplayerpos (\w+):([0-9\:]+)$");
            if (match.Success)
            {
                if (Convert.ToInt32(match.Groups[1].Value) == 2)
                    match = Regex.Match(SocketClient.response, @"^allplayerpos (\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+)$");
                else if (Convert.ToInt32(match.Groups[1].Value) == 4)
                    match = Regex.Match(SocketClient.response, @"^allplayerpos (\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+)$");
                else if (Convert.ToInt32(match.Groups[1].Value) == 6)
                    match = Regex.Match(SocketClient.response, @"^allplayerpos (\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+)$");
                else if (Convert.ToInt32(match.Groups[1].Value) == 8)
                    match = Regex.Match(SocketClient.response, @"^allplayerpos (\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+)$");
                if (match.Success)
                {
                    int counter = 2;
                    for (int i = 0; i < Convert.ToInt32(match.Groups[1].Value); i++)
                    {
                        Player.playGround[Convert.ToInt32(match.Groups[counter + 1].Value), Convert.ToInt32(match.Groups[counter + 2].Value)] = Convert.ToInt32(match.Groups[counter].Value);
                        counter += 3;
                    }
                }
                else
                {
                    //error
                }
            }
            else
            {
                //error
            }
        }
    }
}
