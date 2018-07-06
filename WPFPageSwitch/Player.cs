using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WPFPageSwitch
{
    class Player
    {
        public static int[,] playGround;
        public static int playGroundWidth, playGroundHeight;
        public static string name;
        public static int positionX;
        public static int positionY;
        public static int ID;
        public static bool isYourTurn;
        public static bool Type;
        // Type is False -> is Thief
        // Type is True -> is Cop

        Player()
        {

        }
        public Player(int x, int y, int num)
        {
            playGround = new int[x,y];
            ID = num;
            isYourTurn = true;
            positionX = -1;
            positionY = -1;
        }
    }
}
