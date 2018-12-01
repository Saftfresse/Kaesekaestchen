using ProjectClassLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Discoverer.PeerJoined = ip => Console.WriteLine("JOINED:" + ip);
            Discoverer.PeerLeft = ip => Console.WriteLine("LEFT:" + ip);

            Discoverer.Start();
        }

        private void button_triggerLeft_Click(object sender, EventArgs e)
        {
            if (panel_left.Location.X >= 0)
            {
                panel_left.Location = new Point(0 - panel_left.Width, panel_left.Location.Y);
            }
            else
            {
                panel_left.Location = new Point(0, panel_left.Location.Y);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panel_left.Height = canvas.Height;
        }
    }
}
