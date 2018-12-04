using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class HostNewGame : Form
    {
        public HostNewGame()
        {
            InitializeComponent();
        }

        public string Servername = "";

        private void button1_Click(object sender, EventArgs e)
        {
            Servername = textBox1.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void HostNewGame_Load(object sender, EventArgs e)
        {

        }
    }
}
