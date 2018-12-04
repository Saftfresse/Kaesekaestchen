using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class Player
    {
        public Player()
        {  }

        Guid uid = Guid.NewGuid();
        string name = "New Player";
        Image symbol = Properties.Resources.sym_circle;
        Color color = Color.Green;
        IPEndPoint address = new IPEndPoint(IPAddress.None, 8888);

        public Guid Uid { get => uid; set => uid = value; }
        public string Name { get => name; set => name = value; }
        public Image Symbol { get => symbol; set => symbol = value; }
        public Color Color { get => color; set => color = value; }
        public IPEndPoint Address { get => address; set => address = value; }
    }
}
