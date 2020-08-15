using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Käsekästchen
{
    class ClientInfo
    {
        Guid id;
        string name;
        IPAddress address;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public IPAddress Address { get => address; set => address = value; }
    }
}
