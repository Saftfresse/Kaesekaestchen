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
            draw = new Bitmap(canvas.Height, canvas.Width);
        }

        ClientInfo user;

        List<ClientInfo> clients = new List<ClientInfo>();

        SolidBrush b = new SolidBrush(Color.Red);
        SolidBrush f = new SolidBrush(Color.Black);
        Pen o = new Pen(Color.Black);

        Bitmap draw;

        public ClientInfo User { get => user; set => user = value; }

        private void ClientView_Load(object sender, EventArgs e)
        {

        }

        public async void RefreshClientPos(Guid _id, PointF _loc)
        {
            clients.Find(x => x.Id == _id).Location = _loc;
            await Task.Run(() => paint());
            canvas.Invalidate();
        }

        public async void RefreshClients(List<ClientInfo> _clients)
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
            await Task.Run(() => paint());
            canvas.Invalidate();
        }

        void paint()
        {
            using (Graphics g = Graphics.FromImage(draw))
            {
                g.Clear(Color.White);
                foreach (var item in clients)
                {
                    b.Color = item.Color;
                    foreach (var p in item.Points)
                    {
                        g.FillEllipse(b, new RectangleF(new PointF(p.X - 1, p.Y - 1), new SizeF(5, 5)));
                    }
                    if (item.Location == new PointF(-1, -1))
                    {
                        continue;
                    }
                    g.DrawEllipse(o, new RectangleF(new PointF(item.Location.X - 1, item.Location.Y - 1), new SizeF(6, 6)));
                    g.FillEllipse(b, new RectangleF(new PointF(item.Location.X - 1, item.Location.Y - 1), new SizeF(5, 5)));
                    g.DrawString(item.Name, Font, f, new PointF(item.Location.X + 8, item.Location.Y));
                }
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(draw, 0, 0);
        }
    }
}
