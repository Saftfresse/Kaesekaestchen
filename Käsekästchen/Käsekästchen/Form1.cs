using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Käsekästchen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Server server;

        List<Client> players = new List<Client>();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random r = new Random(5);
            players.Add(new Client(this, Color.FromArgb(r.Next(1,255), r.Next(1, 255), r.Next(1, 255)), tb_server.Text, "New Player " + players.Count));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            server = new Server();
        }
    }
}
