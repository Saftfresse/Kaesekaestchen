﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class Discoverer
    {
        static string MULTICAST_IP = "238.212.223.55"; //Random between 224.X.X.X - 239.X.X.X
        static int MULTICAST_PORT = 2018;    //Random

        static UdpClient _UdpClient;
        static MemoryCache _Peers = new MemoryCache("_PEERS_");

        public static Action<string> PeerJoined = null;
        public static Action<string> PeerLeft = null;

        public static void Start()
        {
            _UdpClient = new UdpClient();
            _UdpClient.Client.Bind(new IPEndPoint(IPAddress.Any, MULTICAST_PORT));
            _UdpClient.JoinMulticastGroup(IPAddress.Parse(MULTICAST_IP));


            Task.Run(() => Receiver());
            Task.Run(() => Sender());
        }

        static void Sender()
        {
            var IamHere = Encoding.UTF8.GetBytes("I AM ALIVE");
            IPEndPoint mcastEndPoint = new IPEndPoint(IPAddress.Parse(MULTICAST_IP), MULTICAST_PORT);

            while (true)
            {
                _UdpClient.Send(IamHere, IamHere.Length, mcastEndPoint);
                Task.Delay(1000).Wait();
            }
        }

        static void Receiver()
        {
            var from = new IPEndPoint(0, 0);
            while (true)
            {
                _UdpClient.Receive(ref from);
                if (_Peers.Add(new CacheItem(from.Address.ToString(), from),
                               new CacheItemPolicy()
                               {
                                   SlidingExpiration = TimeSpan.FromSeconds(20),
                                   RemovedCallback = (x) => { if (PeerLeft != null) PeerLeft(x.CacheItem.Key); }
                               }
                             )
                )
                {
                    if (PeerJoined != null) PeerJoined(from.Address.ToString());
                }

                Console.WriteLine(from.Address.ToString());
            }
        }
    }
}
