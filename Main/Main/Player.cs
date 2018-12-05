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
        int symbol = 0;
        Color color = Color.Green;
        string address = "0.0.0.0";

        public Guid Uid { get => uid; set => uid = value; }
        public string Name { get => name; set => name = value; }
        public int Symbol { get => symbol; set => symbol = value; }
        public Color Color { get => color; set => color = value; }
        public string Address { get => address; set => address = value; }
    }
}
