using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WatsonTcp;

namespace Käsekästchen
{
    public class Client
    {
        Guid id;
        bool local;
        IPAddress connectedTo;

        WatsonTcpClient client;

        Form1 form;

        ClientInfo thisClient;

        public Guid Id { get => id; set => id = value; }
        public bool Local { get => local; set => local = value; }
        public IPAddress ConnectedTo { get => connectedTo; set => connectedTo = value; }
        public Form1 Form { get => form; set => form = value; }


        Timer locTimer = new Timer() { Interval = 10 };

        public ClientView view;

        public Client(Form1 _form, Color _color, string _address = "127.0.0.1", string _name = "Unkwown player")
        {
            form = _form;
            id = Guid.NewGuid();
            local = true;

            thisClient = new ClientInfo() { Name = _name, Id = id, Color = _color };
            view = new ClientView(thisClient);

            client = new WatsonTcpClient(_address, 12000);

            client.ServerConnected += Client_ServerConnected;
            client.ServerDisconnected += Client_ServerDisconnected;
            client.MessageReceived += Client_MessageReceived;

            client.StartAsync();

            Console.WriteLine(thisClient.Name);

            SyncClient();

            view.canvas.MouseMove += Canvas_MouseMove;
            view.canvas.MouseClick += Canvas_MouseClick;
            view.canvas.MouseLeave += Canvas_MouseLeave;

            locTimer.Tick += (s, e) =>
            {
                SyncClient();
            };

            view.FormClosed += View_FormClosed;
            view.Show();
        }

        private void Canvas_MouseLeave(object sender, EventArgs e)
        {
            locTimer.Stop();
        }

        void SyncClient()
        {
            client.SendAsync(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                        new Payload()
                        {
                            Type = Payload.PayloadType.ClientInfo,
                            Data = JsonConvert.SerializeObject(thisClient)
                        })
                    )
                );
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            Log(e.Location.ToString());

            thisClient.Points.Add(e.Location);

            SyncClient();

        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Log(e.Location.ToString());

            locTimer.Start();

            thisClient.Location = e.Location;

            view.Invoke((MethodInvoker)(() =>
            {

            }
            ));
        }

        private void View_FormClosed(object sender, FormClosedEventArgs e)
        {
            Payload p = new Payload() { Type = Payload.PayloadType.ClientCommand, DataType = Payload.PayloadDataType.MsgDisconnect, Data = "Client Disconnected" };
        }

        void Log(string msg)
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

        private void Client_MessageReceived(object sender, MessageReceivedFromServerEventArgs e)
        {
            Payload p = JsonConvert.DeserializeObject<Payload>(Encoding.UTF8.GetString(e.Data));
            Console.WriteLine(p.Data);
            switch (p.Type)
            {
                case Payload.PayloadType.ClientInfo:
                    break;
                case Payload.PayloadType.ClientCommand:
                    break;
                case Payload.PayloadType.ServerCommand:
                    switch (p.DataType)
                    {
                        case Payload.PayloadDataType.MsgDisconnect:
                            Log("Disconnected from Server! Reason: " + p.Data);
                            break;
                        case Payload.PayloadDataType.MsgAcknowledged:
                            Log("Server: Message Received");
                            break;
                        case Payload.PayloadDataType.ClientConnected:
                            Log("Ip if this Client: " + p.Data);
                            thisClient.Address = p.Data;
                            break;
                    }
                    break;
                case Payload.PayloadType.ClientList:
                    Console.WriteLine("test");
                    view.Invoke((MethodInvoker)(() =>
                    {
                        view.RefreshClients(JsonConvert.DeserializeObject<List<ClientInfo>>(p.Data));
                    }
                    ));
                    break;
                default:
                    break;
            }
        }

        private void Client_ServerDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("disc");
        }

        private void Client_ServerConnected(object sender, EventArgs e)
        {
            Console.WriteLine("conn");
            Log("Connected to Server!");
        }

        public void ConnectMulticast()
        {
            Socket socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket socketRec = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress ip = IPAddress.Parse("230.1.2.3");

            socketSend.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));

            socketSend.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

            IPEndPoint endpoint = new IPEndPoint(ip, 12001);
            IPEndPoint endpointRec = new IPEndPoint(ip, 12001);

            socketRec.Bind(endpointRec);

            socketRec.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip, IPAddress.Any));

            byte[] payload = Encoding.ASCII.GetBytes("<CLIENT>" + Dns.GetHostAddresses(Dns.GetHostName())[0].ToString());
            socketSend.Connect(endpoint);
            socketSend.Send(payload, payload.Length, SocketFlags.None);
            socketSend.Close();

            byte[] b = new byte[1024];
            socketRec.Receive(b);
            string str = System.Text.Encoding.ASCII.GetString(b, 0, b.Length);
            Console.WriteLine(str.Trim());
            if (str.Trim().Contains("<HOST>"))
            {
                IPAddress address = IPAddress.Parse(str.Trim().Replace("<HOST>", ""));
                Connect(address);
            }
        }

        public void Connect(IPAddress address)
        {
            byte[] bytes = new byte[1024];

            Console.WriteLine("Connecting...");

            try
            {

                IPEndPoint endpoint = new IPEndPoint(address, 12000);

                Socket sender = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(endpoint);

                    Console.WriteLine("Connection established to " + sender.RemoteEndPoint.ToString());

                    byte[] msg = Encoding.ASCII.GetBytes("Testmessage test 3432432r <EOF>");
                    int bytesSent = sender.Send(msg);

                    int bytesRec = sender.Receive(bytes);

                    Console.WriteLine("Echo: " + Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
