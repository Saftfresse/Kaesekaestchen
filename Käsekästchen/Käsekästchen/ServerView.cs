using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Käsekästchen
{
    public partial class ServerView : Form
    {
        public ServerView()
        {
            InitializeComponent();
        }

        private void ServerView_Load(object sender, EventArgs e)
        {

        }
        public void RefreshClients(List<ClientInfo> _clients)
        {
            lv_players.Items.Clear();
            foreach (var c in _clients)
            {
                ListViewItem lvi = new ListViewItem(c.Name);
                lvi.SubItems.Add(c.Address);
                lv_players.Items.Add(lvi);
            }
        }
    }
}
