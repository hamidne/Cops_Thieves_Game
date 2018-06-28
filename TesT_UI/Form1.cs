using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TesT_UI
{
    public partial class Form1 : Form
    {
        Player cop, thief;
        int col = 5;
        int level = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_set_Click(object sender, EventArgs e)
        {
            if (level == 0)
            {
                flowLayoutPanel1.Controls.Clear();
                int n = int.Parse(textBox1.Text.ToString());
                for (int i = 0; i < n; i++)
                {
                    flowLayoutPanel1.Controls.Add(btn(i));
                }
                cop = new Player(col, Int32.Parse(textBox1.Text) / col, 1);
                cop.Type = true;
                thief = new Player(col, Int32.Parse(textBox1.Text) / col, 2);
                thief.Type = false;
                label1.Text = "Set Police Position : ";
                textBox1.Text = null;
                level++;
            }
            else if (level == 1)
            {
                cop.positionX = Int32.Parse(textBox1.Text) % col;
                cop.positionY = Int32.Parse(textBox1.Text) / col;
                SetPosition(true, Int32.Parse(textBox1.Text) % col, Int32.Parse(textBox1.Text) / col);

                //Control[] Btn = flowLayoutPanel1.Controls.Find("Tile " + textBox1.Text.ToString(), true);
                //Btn[0].Text = "Police";
                //Btn[0].ForeColor = Color.Blue;
                //cop.positionX = Int32.Parse(textBox1.Text) % col;
                //cop.positionY = Int32.Parse(textBox1.Text) / col;
                //cop.playGround[cop.positionX, cop.positionY] = cop.Number;

                label1.Text = "Set Thief Position : ";
                textBox1.Text = null;
                level++;
            }
            else if (level == 2)
            {
                SetPosition(false, Int32.Parse(textBox1.Text) % col, Int32.Parse(textBox1.Text) / col);
                //SetPosition(true, cop.positionX, cop.positionY);

                //Control[] Btn = flowLayoutPanel1.Controls.Find("Tile " + textBox1.Text.ToString(), true);
                //Btn[0].Text = "Thief";
                //Btn[0].ForeColor = Color.Red;
                //thief.positionX = Int32.Parse(textBox1.Text) % col;
                //thief.positionY = Int32.Parse(textBox1.Text) / col;
                //thief.playGround[thief.positionX, thief.positionY] = thief.Number;
                //thief.playGround[cop.positionX, cop.positionY] = cop.Number;
                //cop.playGround[thief.positionX, thief.positionY] = thief.Number;

                label1.Visible = false;
                textBox1.Visible = false;
                btn_set.Visible = false;
                level++;
            }
        }

        Button btn(int i)
        {
            Button b = new Button();
            b.Name = "Tile " + i.ToString();
            b.Width = 62;
            b.Height = 62;
            b.Text = i.ToString();
            b.Click += B_Click;
            return b;
        }
        void B_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            int x = Int32.Parse(b.Text) % col;
            int y = Int32.Parse(b.Text) / col;
            Control[] Btn = flowLayoutPanel1.Controls.Find("Tile " + (thief.positionY * col + thief.positionX).ToString(), true);
            if ((x <= thief.positionX + 1 && x >= thief.positionX - 1) && (y <= thief.positionY + 1 && y >= thief.positionY - 1))
            {
                Btn[0].Text = (thief.positionY * col + thief.positionX).ToString();
                Btn[0].ForeColor = Color.Black;
                cop.playGround[thief.positionX, thief.positionY] = 0;
                thief.playGround[thief.positionX, thief.positionY] = 0;
                SetPosition(false, x, y);
            }
            else
                MessageBox.Show(b.Name.ToString());
        }
        void SetPosition(bool type, int x, int y)
        {
            Control[] Btn = flowLayoutPanel1.Controls.Find("Tile " + (y * col + x).ToString(), true);
            if (type == true)
            {
                Btn[0].Text = "Police";
                Btn[0].ForeColor = Color.Blue;
                cop.positionX = x;
                cop.positionY = y;
            }
            else
            {
                Btn[0].Text = "Thief";
                Btn[0].ForeColor = Color.Red;
                thief.positionX = x;
                thief.positionY = y;
            }

            //Send Request to server To Create new Player with posX,posY and get number and playground response (TO DO)

            if (thief.positionY != -1)
            {
                thief.playGround[thief.positionX, thief.positionY] = thief.Number;
                cop.playGround[thief.positionX, thief.positionY] = thief.Number;
            }
            if (cop.positionY != -1)
            {
                thief.playGround[cop.positionX, cop.positionY] = cop.Number;
                cop.playGround[cop.positionX, cop.positionY] = cop.Number;
            }
        }
    }
}