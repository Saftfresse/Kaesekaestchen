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
        string sender;
        string destination;

        PayloadType type;
        PayloadDataType dataType;

        string data;

        public Guid ClientSender { get => clientSender; set => clientSender = value; }
        public string Sender { get => sender; set => sender = value; }
        public string Destination { get => destination; set => destination = value; }
        public string Data { get => data; set => data = value; }
        public PayloadType Type { get => type; set => type = value; }
        public PayloadDataType DataType { get => dataType; set => dataType = value; }

        public enum PayloadType
        {
            ClientInfo,
            ClientCommand,
            ServerCommand,
            ClientList
        }

        public enum PayloadDataType
        {
            MsgDisconnect,
            MsgAcknowledged,
            ClientConnected,
            ClientMove,
            ClientClick
        }
    }
}
