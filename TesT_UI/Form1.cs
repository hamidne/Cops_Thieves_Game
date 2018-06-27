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
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_set_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            int n = int.Parse(textBox1.Text.ToString());
            for(int i = 0; i < n; i++)
            {
                flowLayoutPanel1.Controls.Add(btn(i));
            }
        }

        Button btn(int i)
        {
            Button b = new Button();
            b.Name = i.ToString();
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
