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

        Server server = new Server();
        Client client = new Client();

        private async void Form1_Load(object sender, EventArgs e)
        {
            await Task.Run(() => server.StartMulticast());
            await Task.Run(() => server.Start());
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() => client.ConnectMulticast());
        }
    }
}
