using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Käsekästchen
{
    public class ClientInfo
    {
        Guid id;
        string name;
        string address;
        PointF location = new PointF(-1, -1);
        List<PointF> points = new List<PointF>();
        Color color;

        public bool Local { get => address == "127.0.0.1" ? true : false; }
        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public PointF Location { get => location; set => location = value; }
        public List<PointF> Points { get => points; set => points = value; }
        public Color Color { get => color; set => color = value; }
    }
}
