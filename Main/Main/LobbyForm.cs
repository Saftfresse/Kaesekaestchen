using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class LobbyForm : Form
    {
        string sName;
        bool host;
        Player admin;
        List<Player> lobbyPlayers = new List<Player>();
        UdpClient Server = new UdpClient(8888);
        public LobbyForm(string _serverName, bool _host, Player _admin)
        {
            admin = _admin;
            lobbyPlayers.Add(admin);
            sName = _serverName;
            host = _host;
            InitializeComponent();
        }

        public byte[] ResponseData { get; private set; }

        private async void LobbyForm_Load(object sender, EventArgs e)
        {
            label3.Text = sName;
            if (host) await Task.Run(() => server());
        }

        void server()
        {
            var ResponseData = Encoding.ASCII.GetBytes(sName);

            while (true)
            {
                var ClientEp = new IPEndPoint(IPAddress.Any, 0);
                var ClientRequestData = Server.Receive(ref ClientEp);
                var ClientRequest = Encoding.ASCII.GetString(ClientRequestData);
                Server.Client.IOControl(-1744830452, new byte[] { 0 }, new byte[] { 0 });
                if (ClientRequest.Length > 0)
                {
                    Console.WriteLine(ClientRequest);
                    Player p = JsonConvert.DeserializeObject<Player>(ClientRequest);
                    p.Address = ClientEp.Address.ToString();
                    if (!lobbyPlayers.Contains(p)) lobbyPlayers.Add(p);
                    Console.WriteLine(p.Address);
                }
                listBox1.BeginInvoke((MethodInvoker)delegate ()
                {
                    listBox1.Items.Clear();
                    foreach (Player item in lobbyPlayers)
                    {
                        if (item == admin)
                        {
                            listBox1.Items.Add(item.Name + " [Host]");
                        }
                        else
                        {
                            listBox1.Items.Add(item.Name);
                        }
                    }
                });
                Server.Send(ResponseData, ResponseData.Length, ClientEp);
            }
        }

        void updateClients()
        {
            foreach (var item in lobbyPlayers)
            {
                if (item != admin)
                {
                    var ClientEp = new IPEndPoint(IPAddress.Parse(item.Address), 0);
                    Server.Send(ResponseData, ResponseData.Length, ClientEp);
                }
            }
        }

        void updateList()
        {
            listBox1.Items.Clear();
            foreach (Player item in lobbyPlayers)
            {
                if (item == admin)
                {
                    listBox1.Items.Add(item.Name + " [Host]");
                }
                else
                {
                    listBox1.Items.Add(item.Name);
                }
            }
        }
    }
}
