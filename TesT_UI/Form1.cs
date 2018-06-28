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
                cop = new Player(5, Int32.Parse(textBox1.Text) / 5, 1);
                cop.Type = true;
                thief = new Player(5, Int32.Parse(textBox1.Text) / 5, 2);
                thief.Type = true;
                label1.Text = "Set Police Position : ";
                textBox1.Text = null;
                level++;
            }
            else if (level == 1)
            {
                Control[] Btn = flowLayoutPanel1.Controls.Find("Tile " + textBox1.Text.ToString(), true);
                Btn[0].Text = "Police";
                Btn[0].ForeColor = Color.Blue;
                //Send Request to server To Create new Player with posX,posY and get number and playground response (TO DO)
                cop.positionX = Int32.Parse(textBox1.Text) % 5;
                cop.positionY = Int32.Parse(textBox1.Text) / 5;
                cop.playGround[cop.positionX, cop.positionY] = cop.Number;

                label1.Text = "Set Thief Position : ";
                textBox1.Text = null;
                level++;
            }
            else if (level == 2)
            {
                Control[] Btn = flowLayoutPanel1.Controls.Find("Tile " + textBox1.Text.ToString(), true);
                Btn[0].Text = "Thief";
                //Send Request to server To Create new Player with posX,posY and get number and playground response (TO DO)
                Btn[0].ForeColor = Color.Red;
                thief.positionX = Int32.Parse(textBox1.Text) % 5;
                thief.positionY = Int32.Parse(textBox1.Text) / 5;
                thief.playGround[thief.positionX, thief.positionY] = thief.Number;
                thief.playGround[cop.positionX, cop.positionY] = cop.Number;
                cop.playGround[thief.positionX, thief.positionY] = thief.Number;

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
        void B_Click(object sender,EventArgs e)
        {
            Button b = (Button)sender;
            MessageBox.Show(b.Name.ToString());
        }
    }
}