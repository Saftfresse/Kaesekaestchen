using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Käsekästchen
{
    public class Server
    {
        IPAddress address;
        string name;

        List<ClientInfo> connectedClients = new List<ClientInfo>();

        public IPAddress Address { get => address; set => address = value; }
        public string Name { get => name; set => name = value; }
        internal List<ClientInfo> ConnectedClients { get => connectedClients; set => connectedClients = value; }

        public static IPAddress GetIPAddress(string hostName)
        {
            Ping ping = new Ping();
            var replay = ping.Send(hostName);

            if (replay.Status == IPStatus.Success)
            {
                return replay.Address;
            }
            return null;
        }

        public static IEnumerable<IPAddress> GetAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip).ToList();
        }

        public void StartMulticast()
        {
            Socket socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket socketRec = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress multiIp = IPAddress.Parse("230.1.2.3");
            IPAddress localIp = GetAddresses().ElementAt(2);
            

            socketSend.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multiIp));

            socketSend.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

            IPEndPoint endpoint = new IPEndPoint(multiIp, 12001);
            //IPEndPoint endpointRec = new IPEndPoint(ip, 12001);

            socketRec.Bind(endpoint);

            socketRec.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multiIp, IPAddress.Any));


            while (true)
            {
                byte[] b = new byte[1024];
                socketRec.Receive(b);
                string str = System.Text.Encoding.ASCII.GetString(b, 0, b.Length);
                Console.WriteLine(str.Trim());
                if (str.Trim().Contains("<CLIENT>"))
                {
                    byte[] payload = Encoding.ASCII.GetBytes("<HOST>" + Dns.GetHostAddresses(Dns.GetHostName())[0].ToString());
                    socketSend.Connect(endpoint);
                    socketSend.Send(payload, payload.Length, SocketFlags.None);
                    socketSend.Close();
                }
            }
        }

        public void Start()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            address = host.AddressList[0];
            IPEndPoint endpoint = new IPEndPoint(address, 12000);

            try
            {
                Socket listener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(endpoint);

                listener.Listen(10);

                Console.WriteLine("Waiting for connection...");
                Socket handler = listener.Accept();

                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1) 
                    {
                        break;
                    }
                }

                Console.WriteLine("REC: " + data);
                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Stop()
        {

        }
    }
}
