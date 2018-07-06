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
            worker.RunWorkerAsync();
            //throw new NotImplementedException();
        }

        private void WaitForResponse(object sender, DoWorkEventArgs e)
        {
            while (SocketClient.response == "") ;
            if (SocketClient.response.StartsWith("allplayerpos"))
                SetAllPlayersPosition(SocketClient.response);
            else if (SocketClient.response.StartsWith("moved"))
                SetChangedPlayerPosition(SocketClient.response);
            
        }

        private void SetChangedPlayerPosition(string response)
        {
            Match match = Regex.Match(response, @"^moved (\w+):(\w+):(\w+):(\w+)$");
            if (match.Success)
            {
                int id = Convert.ToInt32(match.Groups[1].Value);
                int x = Convert.ToInt32(match.Groups[2].Value);
                int y = Convert.ToInt32(match.Groups[3].Value);
                bool turn = Convert.ToBoolean(match.Groups[4].Value);
                for (int i = 0; i < Player.playGroundWidth; i++)
                {
                    for (int j = 0; j < Player.playGroundHeight; j++)
                    {
                        if (Player.playGround[i, j] == id)
                            Player.playGround[i, j] = 0;
                    }
                }
                Player.playGround[x, y] = id;
                Player.isYourTurn = turn;
            }
            else
            {
                //error
            }
        }

        private void SetAllPlayersPosition(string response)
        {
            Match match = Regex.Match(response, @"^allplayerpos (\w+):([0-9\:]+)$");
            if (match.Success)
            {
                if (Convert.ToInt32(match.Groups[1].Value) == 2)
                    match = Regex.Match(response, @"^allplayerpos (\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+)$");
                else if (Convert.ToInt32(match.Groups[1].Value) == 4)
                    match = Regex.Match(response, @"^allplayerpos (\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+)$");
                else if (Convert.ToInt32(match.Groups[1].Value) == 6)
                    match = Regex.Match(response, @"^allplayerpos (\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+)$");
                else if (Convert.ToInt32(match.Groups[1].Value) == 8)
                    match = Regex.Match(response, @"^allplayerpos (\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+):(\w+)$");
                if (match.Success)
                {
                    int counter = 2;
                    for (int i = 0; i < Convert.ToInt32(match.Groups[1].Value); i++)
                    {
                        Player.playGround[Convert.ToInt32(match.Groups[counter + 1].Value), Convert.ToInt32(match.Groups[counter + 2].Value)] = Convert.ToInt32(match.Groups[counter].Value);
                        counter += 3;
                    }
                    SocketClient.response = "";

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
            SocketClient.response = "";

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SocketClient.SendString("move " + Player.ID + ":" + xTextBox.Text + ":" + yTextBox.Text);
            Player.positionX = Convert.ToInt32(xTextBox.Text);
            Player.positionY = Convert.ToInt32(yTextBox.Text);

        }
    }
}
