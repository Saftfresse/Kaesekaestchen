using WatsonTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Käsekästchen
{
    public class Server
    {
        IPAddress address;
        string name;

        List<ClientInfo> connectedClients = new List<ClientInfo>();

        bool inLobby = false;

        public IPAddress Address { get => address; set => address = value; }
        public string Name { get => name; set => name = value; }
        internal List<ClientInfo> ConnectedClients { get => connectedClients; set => connectedClients = value; }

        MulticastListener listener = new MulticastListener();
        MulticastSender sender = new MulticastSender();

        ServerView view;

        WatsonTcpServer server;

        public Server()
        {
            view = new ServerView();
            server = new WatsonTcpServer("192.168.178.42", 12000);

            server.ClientConnected += Server_ClientConnected;
            server.ClientDisconnected += Server_ClientDisconnected;
            server.MessageReceived += Server_MessageReceived;

            server.StartAsync();

            view.FormClosing += View_FormClosing;
            view.Show();
        }

        private void View_FormClosing(object sender, FormClosingEventArgs e)
        {
            Payload pb = new Payload() { Data = "Server Shutdown", Type = Payload.PayloadType.ServerCommand, DataType = Payload.PayloadDataType.MsgDisconnect };
            Broadcast(pb);
            foreach (var item in connectedClients)
            {
                server.DisconnectClient(item.Address);
            }
            
        }

        private void Server_MessageReceived(object sender, MessageReceivedFromClientEventArgs e)
        {
            //
            // Types
            // 000 = Payload
            // 001 = ClientMove
            //  data = guid;posX:posY
            //
            object type = "000";
            e.Metadata.TryGetValue("type", out type);
            switch (type)
            {
                case "payload":
                    Payload p = JsonConvert.DeserializeObject<Payload>(Encoding.UTF8.GetString(e.Data));
                    Log("REC " + p.Type);
                    switch (p.Type)
                    {
                        case Payload.PayloadType.ClientInfo:
                            ClientInfo ci = JsonConvert.DeserializeObject<ClientInfo>(p.Data);
                            ci.Address = e.IpPort;
                            if (connectedClients.Where(x => x.Id == ci.Id).Count() <= 0)
                            {
                                connectedClients.Add(ci);
                                Log(ci.Id.ToString() + " - " + ci.Name + " Connected!");
                            }
                            else
                            {
                                connectedClients[connectedClients.FindIndex(x => x.Id == ci.Id)] = ci;
                            }
                            RefreshClients();
                            break;
                        case Payload.PayloadType.ClientCommand:
                            switch (p.DataType)
                            {
                                case Payload.PayloadDataType.ClientMove:

                                    break;
                                case Payload.PayloadDataType.ClientClick:
                                    break;
                            }
                            break;
                        case Payload.PayloadType.ServerCommand:

                            break;
                    }
                    break;
                case "move":
                    string[] spl = Encoding.UTF8.GetString(e.Data).Split(';');
                    connectedClients.Find(x => x.Id == Guid.Parse(spl[0])).Location = new System.Drawing.PointF(float.Parse(spl[1].Split(':')[0]), float.Parse(spl[1].Split(':')[1]));
                    Broadcast(Encoding.UTF8.GetString(e.Data), "move");
                    break;
                default:
                    break;
            }
            
            Payload pb = new Payload() { Data = "Received", Type = Payload.PayloadType.ServerCommand, DataType = Payload.PayloadDataType.MsgAcknowledged };
            server.SendAsync(e.IpPort, JsonConvert.SerializeObject(pb));
        }

        private void Server_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Log("Client Disc: " + e.IpPort);
            RefreshClients();
        }

        private void Server_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Payload pb = new Payload() { Data = e.IpPort, Type = Payload.PayloadType.ServerCommand, DataType = Payload.PayloadDataType.ClientConnected };
            server.SendAsync(e.IpPort, JsonConvert.SerializeObject(pb));
            Log("Client Conn: " + e.IpPort);
        }
        void RefreshClients()
        {
            view.Invoke((MethodInvoker)(() =>
                {
                    view.RefreshClients(connectedClients);
                } 
            ));
            string list = JsonConvert.SerializeObject(connectedClients);
            Payload p = new Payload() { Data = list, Type = Payload.PayloadType.ClientList };
            Broadcast(p);
        }

        void Broadcast(Payload _p)
        {
            string payload = JsonConvert.SerializeObject(_p);
            var meta = new Dictionary<object, object>();
            meta.Add("type", "payload");
            foreach (var c in connectedClients)
            {
                server.SendAsync(c.Address, meta, payload);
            }
        }

        void Broadcast(string _data, string type)
        {
            string payload = _data;
            var meta = new Dictionary<object, object>();
            meta.Add("type", type);
            foreach (var c in connectedClients)
            {
                server.SendAsync(c.Address, meta, payload);
                
            }
        }

        void Log(string msg)
        {
            try
            {
                if (view.InvokeRequired)
                {
                    view.Invoke((MethodInvoker)(() =>
                    {
                        view.tb_log.Text += string.Format("{0} > {1}{2}", DateTime.Now.ToLongTimeString(), msg, Environment.NewLine);
                    }
                    ));
                }
                else
                {
                    view.tb_log.Text += string.Format("{0} > {1}{2}", DateTime.Now.ToLongTimeString(), msg, Environment.NewLine);
                }
            }
            catch (Exception)
            {
            }
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
