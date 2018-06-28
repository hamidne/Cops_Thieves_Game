using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesT_UI
{
    class Player
    {
        public int[,] playGround;
        public int positionX;
        public int positionY;
        public int Number;
        public bool isYourTurn;
        public bool Type;
        // Type is False -> is Thief
        // Type is True -> is Cop

        Player()
        {

        }
        public Player(int x, int y, int num)
        {
            playGround = new int[x,y];
            Number = num;
            isYourTurn = true;
            positionX = -1;
            positionY = -1;
        }
    }
}
