using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Käsekästchen
{
    public class Payload
    {
        Guid clientSender;
        IPAddress sender;
        IPAddress destination;

        string data;

        public Guid ClientSender { get => clientSender; set => clientSender = value; }
        public IPAddress Sender { get => sender; set => sender = value; }
        public IPAddress Destination { get => destination; set => destination = value; }
        public string Data { get => data; set => data = value; }
    }
}
