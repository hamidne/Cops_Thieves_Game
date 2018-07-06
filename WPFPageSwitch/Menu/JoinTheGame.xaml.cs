using System;
using System.Collections.Generic;
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
    /// Interaction logic for JoinTheGame.xaml
    /// </summary>
    public partial class JoinTheGame : UserControl
    {
        public JoinTheGame()
        {
            InitializeComponent();
            SocketClient.SendString("want_to_join");
            HandleResponse();
        }
        private void HandleResponse()
        {
            while (SocketClient.response == "") ;
            if (SocketClient.response== "nothing")
            {
                JoinCopBtn.Visibility = Visibility.Hidden;
                JoinThiefBtn.Visibility = Visibility.Hidden;
            }
            else if (SocketClient.response == "onlythief")
            {
                JoinCopBtn.Visibility = Visibility.Hidden;

            }
            else if (SocketClient.response == "onlycop")
            {
                JoinThiefBtn.Visibility = Visibility.Hidden;

            }
            else if (SocketClient.response == "both")
            {
                //Nothing
            }
            else
            {
                //Error
            }
               

            SocketClient.response = "";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new CreateGameAndJoin());
        }

        private void SetUpPlayGround(bool type, int width, int height)
        {
            Player.Type = type;
            Player.name = Name.Text;
            Player.playGroundWidth = width;
            Player.playGroundHeight = height;
            Player.playGround = new int[Player.playGroundWidth, Player.playGroundHeight];
            for (int i = 0; i < Player.playGroundWidth; i++)
            {
                for (int j = 0; j < Player.playGroundHeight; j++)
                {
                    Player.playGround[i, j] = 0;
                }
            }

        }

        private void JoinCopBtn_Click(object sender, RoutedEventArgs e)
        {
            SocketClient.SendString("join " + Name.Text + ":cop");
            SecondHandleResponse(true);

        }


        private void JoinThiefBtn_Click(object sender, RoutedEventArgs e)
        {
            SocketClient.SendString("join " + Name.Text + ":thief");
            SecondHandleResponse(false);

        }
        private void SecondHandleResponse(bool type)
        {
            while (SocketClient.response == "") ;
            Match match = Regex.Match(SocketClient.response, @"^joined (\w+):(\w+):(\w+):(\w+):(\w+):(\w+)$");
            if (match.Success)
            {
                SetUpPlayGround(type, Convert.ToInt32(match.Groups[5].Value), Convert.ToInt32(match.Groups[6].Value));
                Player.ID = Convert.ToInt32(match.Groups[1].Value);
                Player.positionX = Convert.ToInt32(match.Groups[2].Value);
                Player.positionY = Convert.ToInt32(match.Groups[3].Value);
                Player.playGround[Player.positionX, Player.positionY] = Player.ID;
                Player.isYourTurn = Convert.ToBoolean(match.Groups[4].Value);
                SocketClient.response = "";
                Switcher.Switch(new GamePlay());
            }
            else
            {
                //Print Error
            }

            SocketClient.response = "";
        }
    }

}
