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
    /// Interaction logic for SetGameInformation.xaml
    /// </summary>
    public partial class SetGameInformation : UserControl
    {
        public SetGameInformation()
        {
            InitializeComponent();
        }

        private void SetUpPlayGround(bool type)
        {
            Player.Type = type;
            Player.name = Name.Text;
            Player.playGroundWidth = Convert.ToInt32(PlaygroundX.Text);
            Player.playGroundHeight = Convert.ToInt32(PlaygroundY.Text);
            Player.playGround = new int[Player.playGroundWidth, Player.playGroundHeight];
            for (int i = 0; i < Player.playGroundWidth; i++)
            {
                for (int j = 0; j < Player.playGroundHeight; j++)
                {
                    Player.playGround[i, j] = 0;
                }
            }
            
        }

        private void CreateGameCopBtn_Click(object sender, RoutedEventArgs e)
        {
            SetUpPlayGround(true);
            SocketClient.SendString("create "+Name.Text+":"+NumOfPlayers.Text+":"+PlaygroundX.Text+":"+PlaygroundY.Text+":cop" );
            HandleResponse();

        }
        private void CreateGameThiefBtn_Click(object sender, RoutedEventArgs e)
        {
            SetUpPlayGround(false);
            SocketClient.SendString("create " + Name.Text + ":" + NumOfPlayers.Text + ":" + PlaygroundX.Text + ":" + PlaygroundY.Text + ":thief");
            HandleResponse();
        }


        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void HandleResponse()
        {
            while (SocketClient.response == "") ;
            Match match = Regex.Match(SocketClient.response, @"^created (\w+):(\w+):(\w+):(\w+)$");
            if (match.Success)
            {
                Player.ID = Convert.ToInt32(match.Groups[1].Value);
                Player.positionX = Convert.ToInt32(match.Groups[2].Value);
                Player.positionY = Convert.ToInt32(match.Groups[3].Value);
                Player.isYourTurn = Convert.ToBoolean(match.Groups[4].Value);
                SocketClient.response = "";
                //Switcher.Switch();
            }
            else
            {
                //PRINT ERROR
            }
        }

          }
}
