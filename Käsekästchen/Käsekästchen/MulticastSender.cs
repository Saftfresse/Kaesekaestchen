using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Käsekästchen
{
    class MulticastSender : Multicast
    {
        IPAddress mcastAddress;
        int mcastPort;
        Socket mcastSocket;

        public IPAddress McastAddress { get => mcastAddress; set => mcastAddress = value; }
        public int McastPort { get => mcastPort; set => mcastPort = value; }
        public Socket McastSocket { get => mcastSocket; set => mcastSocket = value; }

        public void JoinMulticastGroup()
        {
            try
            {
                // Create a multicast socket.
                McastSocket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Dgram,
                                         ProtocolType.Udp);

                // Get the local IP address used by the listener and the sender to
                // exchange multicast messages.
                IPAddress localIPAddr = GetAddresses().ElementAt(2);

                // Create an IPEndPoint object.
                IPEndPoint IPlocal = new IPEndPoint(localIPAddr, 0);

                // Bind this endpoint to the multicast socket.
                McastSocket.Bind(IPlocal);

                // Define a MulticastOption object specifying the multicast group
                // address and the local IP address.
                // The multicast group address is the same as the address used by the listener.
                MulticastOption mcastOption;
                mcastOption = new MulticastOption(McastAddress, localIPAddr);

                McastSocket.SetSocketOption(SocketOptionLevel.IP,
                                            SocketOptionName.AddMembership,
                                            mcastOption);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }
        }

        public void BroadcastMessage(string message)
        {
            IPEndPoint endPoint;

            try
            {
                //Send multicast packets to the listener.
                endPoint = new IPEndPoint(McastAddress, McastPort);
                McastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes(message), endPoint);
                Console.WriteLine("Multicast data sent.....");
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }

            McastSocket.Close();
        }
    }
}
