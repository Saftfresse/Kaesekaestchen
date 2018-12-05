using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class JoinGame : Form
    {
        Player player;
        public JoinGame(Player p)
        {
            player = p;
            InitializeComponent();
        }
        

        void client()
        {
            var Client = new UdpClient();
            var RequestData = Encoding.ASCII.GetBytes("");
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);

            Client.EnableBroadcast = true;
            Client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));
             
            for (int i = 0; i < 10; i++)
            {
                var ServerResponseData = Client.Receive(ref ServerEp);
                var ServerResponse = Encoding.ASCII.GetString(ServerResponseData);

                ListViewItem lvi = new ListViewItem();
                lvi.Text = ServerResponse + ServerEp.Address;
                lvi.Tag = ServerEp;

                listBox1.Invoke((MethodInvoker)delegate { listBox1.Items.Add(lvi); });
            }
            Client.Close();
        }

        void addPlayer()
        {

        }

        private async void JoinGame_Load(object sender, EventArgs e)
        {
            await Task.Run(() => client());
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            var Client = new UdpClient();
            var RequestData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(player));
            var ServerEp = new IPEndPoint(((IPEndPoint)((ListViewItem)listBox1.SelectedItem).Tag).Address, 0);
            
            Client.Send(RequestData, RequestData.Length, new IPEndPoint(ServerEp.Address, 8888));

            //for (int i = 0; i < 10; i++)
            //{
            //    var ServerResponseData = Client.Receive(ref ServerEp);
            //    var ServerResponse = Encoding.ASCII.GetString(ServerResponseData);

            //    ListViewItem lvi = new ListViewItem();
            //    lvi.Text = ServerResponse;
            //    lvi.Tag = ServerEp;

            //    listBox1.Invoke((MethodInvoker)delegate { listBox1.Items.Add(lvi); });
            //}
            Client.Close();
        }
    }
}
