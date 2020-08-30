using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Käsekästchen
{
    class MulticastListener : Multicast
    {
        IPAddress mcastAddress;
        int mcastPort;
        Socket mcastSocket;
        MulticastOption mcastOption;

        public IPAddress McastAddress { get => mcastAddress; set => mcastAddress = value; }
        public int McastPort { get => mcastPort; set => mcastPort = value; }
        public Socket McastSocket { get => mcastSocket; set => mcastSocket = value; }
        public MulticastOption McastOption { get => mcastOption; set => mcastOption = value; }

        private void MulticastOptionProperties()
        {
            Console.WriteLine("Current multicast group is: " + McastOption.Group);
            Console.WriteLine("Current multicast local address is: " + McastOption.LocalAddress);
        }


        public void StartMulticast()
        {

            try
            {
                McastSocket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Dgram,
                                         ProtocolType.Udp);

                Console.Write("Enter the local IP address: ");

                IPAddress localIPAddr = GetAddresses().ElementAt(2);

                //IPAddress localIP = IPAddress.Any;
                EndPoint localEP = (EndPoint)new IPEndPoint(localIPAddr, McastPort);

                McastSocket.Bind(localEP);


                // Define a MulticastOption object specifying the multicast group
                // address and the local IPAddress.
                // The multicast group address is the same as the address used by the server.
                McastOption = new MulticastOption(McastAddress, localIPAddr);

                McastSocket.SetSocketOption(SocketOptionLevel.IP,
                                            SocketOptionName.AddMembership,
                                            McastOption);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void ReceiveBroadcastMessages()
        {
            bool done = false;
            byte[] bytes = new Byte[1024];
            IPEndPoint groupEP = new IPEndPoint(McastAddress, McastPort);
            EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

            try
            {
                while (!done)
                {
                    Console.WriteLine("Waiting for multicast packets.......");
                    Console.WriteLine("Enter ^C to terminate.");

                    McastSocket.ReceiveFrom(bytes, ref remoteEP);

                    Console.WriteLine("Received broadcast from {0} :\n {1}\n",
                      groupEP.ToString(),
                      Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                }

                McastSocket.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
