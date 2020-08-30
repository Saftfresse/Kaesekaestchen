using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Käsekästchen
{
    public partial class ClientView : Form
    {
        public ClientView(ClientInfo _user)
        {
            InitializeComponent();
            User = _user;
        }

        ClientInfo user;

        List<ClientInfo> clients = new List<ClientInfo>();

        public ClientInfo User { get => user; set => user = value; }

        private void ClientView_Load(object sender, EventArgs e)
        {

        }

        public void RefreshClients(List<ClientInfo> _clients)
        {
            clients = _clients;
            lv_players.Items.Clear();
            foreach (var c in clients)
            {
                ListViewItem lvi = new ListViewItem(c.Name);
                lvi.SubItems.Add(c.Address);
                if (c.Id == user.Id) lvi.BackColor = Color.LightGray;
                lv_players.Items.Add(lvi);
            }
            canvas.Invalidate();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(Color.Red);
            SolidBrush f = new SolidBrush(Color.Black);
            Pen o = new Pen(Color.Black);
            foreach (var item in clients)
            {
                b.Color = item.Color;
                foreach (var p in item.Points)
                {
                    e.Graphics.FillEllipse(b, new RectangleF(new PointF(p.X - 1, p.Y - 1), new SizeF(5, 5)));
                }
                if (item.Location == new PointF(-1, -1))
                {
                    continue;
                }
                e.Graphics.DrawEllipse(o, new RectangleF(new PointF(item.Location.X - 1, item.Location.Y - 1), new SizeF(6, 6)));
                e.Graphics.FillEllipse(b, new RectangleF(new PointF(item.Location.X - 1, item.Location.Y - 1), new SizeF(5, 5)));
                e.Graphics.DrawString(item.Name, Font, f, new PointF(item.Location.X + 8, item.Location.Y));
            }
        }
    }
}
