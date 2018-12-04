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
        public LobbyForm(string _serverName, bool _host)
        {
            sName = _serverName;
            host = _host;
            InitializeComponent();
        }

        private async void LobbyForm_Load(object sender, EventArgs e)
        {
            label3.Text = sName;
            if (host) await Task.Run(() => server());
        }

        void server()
        {
            var Server = new UdpClient(8888);
            var ResponseData = Encoding.ASCII.GetBytes(sName);

            while (true)
            {
                var ClientEp = new IPEndPoint(IPAddress.Any, 0);
                var ClientRequestData = Server.Receive(ref ClientEp);
                var ClientRequest = Encoding.ASCII.GetString(ClientRequestData);
                if (ClientRequest.Length > 0)
                {
                    Console.WriteLine(ClientRequest);
                    //Player p = JsonConvert.DeserializeObject<Player>(ClientRequest);
                    //Console.WriteLine(p.Address);
                }
                listBox1.BeginInvoke((MethodInvoker)delegate ()
                {
                    listBox1.Items.Add(string.Format("Recived {0} from {1}, sending response", ClientRequest, ClientEp.Address.ToString()));
                });
                Server.Send(ResponseData, ResponseData.Length, ClientEp);
            }
        }
    }
}
