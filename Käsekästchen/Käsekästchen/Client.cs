using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Käsekästchen
{
    public class Client
    {
        Guid id;
        string name;
        IPAddress address;
        IPAddress connectedTo;

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
