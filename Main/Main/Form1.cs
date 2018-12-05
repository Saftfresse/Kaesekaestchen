using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Gridboard board = new Gridboard();
        Bitmap screen;
        Bitmap overlay;

        List<Image> symbols = new List<Image>() {
            Properties.Resources.sym_circle,
            Properties.Resources.sym_cross,
            Properties.Resources.sym_flash,
            Properties.Resources.sym_star };

        Size canvasSize = new Size();
        Point MPos = new Point();
        int cellSize = 30;
        Point pZero = new Point(5,5);
        Player player_1 = new Player() { Name = "Current", Symbol = 2 };
        Player player_2 = new Player() { Name = "Opponent", Symbol = 3 };

        MapCollection maps = new MapCollection();

        Gridcell upperCell(Gridcell start)
        {
            return board.Cells.Find(a => a.Bounds.Y == start.Bounds.Y - start.Bounds.Height && a.Bounds.X == start.Bounds.X);
        }

        Gridcell leftCell(Gridcell start)
        {
            return board.Cells.Find(a => a.Bounds.X == start.Bounds.X - start.Bounds.Width && a.Bounds.Y == start.Bounds.Y);
        }

        Gridcell getCellOnCursor(Point p)
        {
            Gridcell g = null;
            foreach (var item in board.Cells)
            {
                if (item.Bounds.Contains(p))
                {
                    g = item;
                }
            }
            return g;
        }

        Point farthestPoint(int dir, Point loc)
        {
            Point p = loc;
            switch (dir)
            {
                case 0:
                    foreach (var item in board.Cells)
                    {
                        if (item.Bounds.Location.X == p.X)
                        {
                            if (item.Bounds.Y < p.Y && item.Bounds.Y > p.Y - cellSize - 5)
                            {
                                p = item.Bounds.Location;
                            }
                        }
                    }
                    break;
                case 1:
                    foreach (var item in board.Cells)
                    {
                        if (item.Bounds.Location.Y == loc.Y && item.Bounds.Location.X > p.X && item.Bounds.X < p.X + cellSize + 5)
                        {
                            p = item.Bounds.Location;
                        }
                    }
                    break;
                case 2:
                    foreach (var item in board.Cells)
                    {
                        if (item.Bounds.Location.X == loc.X && item.Bounds.Location.Y > p.Y && item.Bounds.Y < p.Y + cellSize + 5)
                        {
                            p = item.Bounds.Location;
                        }
                    }
                    break;
                case 3:
                    foreach (var item in board.Cells)
                    {
                        if (item.Bounds.Location.Y == loc.Y && item.Bounds.Location.X < p.X && item.Bounds.X > p.X - cellSize - 5)
                        {
                            p = item.Bounds.Location;
                        }
                    }
                    break;
            }
            return p;
        }

        bool isCheckPossible()
        {
            bool ret = false;
            foreach (var item in board.Cells)
            {
                int count = 0;

                if (upperCell(item) != null && upperCell(item).Lines[1].Set)
                {
                    count++;
                }
                if (leftCell(item) != null && leftCell(item).Lines[0].Set)
                {
                    count++;
                }
                count += item.OutlineDirs.Count;
                if (item.Lines[0].Set) count++;
                if (item.Lines[1].Set) count++;
                if (count >= 4 && !item.Taken)
                {
                    Console.WriteLine("Check: available " + count);
                    ret = true;
                }
            }
            return ret;
        }

        void applyChecks()
        {
            foreach (var item in board.Cells)
            {
                bool[] ct = new bool[4];
                int count = 0;
                List<Gridcell.OutlineDirection> dirs = new List<Gridcell.OutlineDirection>() { Gridcell.OutlineDirection.North, Gridcell.OutlineDirection.East, Gridcell.OutlineDirection.South, Gridcell.OutlineDirection.West };
                Gridcell.OutlineDirection open = Gridcell.OutlineDirection.North;
                if (upperCell(item) != null && upperCell(item).Lines[1].Set)
                {
                    ct[0] = true;
                    count++;
                    dirs.Remove(Gridcell.OutlineDirection.North);
                }
                if (leftCell(item) != null && leftCell(item).Lines[0].Set)
                {
                    ct[1] = true;
                    count++;
                    dirs.Remove(Gridcell.OutlineDirection.East);
                }
                if (item.Lines[0].Set) {
                    ct[2] = true;
                    count++;
                    dirs.Remove(Gridcell.OutlineDirection.South);
                }
                if (item.Lines[1].Set) {
                    ct[3] = true;
                    count++;
                    dirs.Remove(Gridcell.OutlineDirection.West);
                }
                int c = item.OutlineDirs.Count + count;
                if (c == 4)
                {
                    Console.WriteLine("Set: " +c);
                    //if (upperCell(item) != null) upperCell(item).Lines[1].Set = true;
                    //item.Lines[0].Set = true;
                    //item.Lines[1].Set = true;
                    //if (leftCell(item) != null) leftCell(item).Lines[0].Set = true;

                    item.Taken = true;
                    item.PlayerId = player_1.Uid;
                }
            }
        }

        void draw()
        {
            drawGrid();
            drawOnGrid();
            canvas.Invalidate();
        }

        void drawGrid()
        {
            int widthBufX = 0, widthBufY = 0;
            int left = 0, bottom = 0;
            Pen rec = new Pen(Color.DimGray);
            Pen outline = new Pen(Color.WhiteSmoke, 1);
            Point p1 = new Point();
            using (var g = Graphics.FromImage(screen))
            {
                g.Clear(Color.FromArgb(19, 19, 19));
                foreach (var item in board.Cells)
                {
                    g.DrawRectangle(rec, item.Bounds);

                    // Ränder
                    if (farthestPoint(0, item.Bounds.Location) == item.Bounds.Location)
                    {
                        g.DrawLine(outline, item.Bounds.X, item.Bounds.Y, item.Bounds.X + item.Bounds.Width + widthBufX, item.Bounds.Y);
                        item.OutlineDirs.Add(Gridcell.OutlineDirection.North);
                    }
                    if (farthestPoint(1, item.Bounds.Location) == item.Bounds.Location)
                    {
                        g.DrawLine(outline, item.Bounds.X + item.Bounds.Width, item.Bounds.Y, item.Bounds.X + item.Bounds.Width, item.Bounds.Y + item.Bounds.Height + widthBufX);
                        item.OutlineDirs.Add(Gridcell.OutlineDirection.East);
                        left = left < item.Bounds.X + item.Bounds.Width ? item.Bounds.X + item.Bounds.Width : left;
                        if (item.GridLocation.X > p1.X) p1.X = item.GridLocation.X;
                    }
                    if (farthestPoint(2, item.Bounds.Location) == item.Bounds.Location)
                    {
                        g.DrawLine(outline, item.Bounds.X + item.Bounds.Width, item.Bounds.Y + item.Bounds.Height, item.Bounds.X - widthBufY, item.Bounds.Y + item.Bounds.Height);
                        item.OutlineDirs.Add(Gridcell.OutlineDirection.South);
                        bottom = bottom < item.Bounds.Y + item.Bounds.Height ? item.Bounds.Y + item.Bounds.Height : bottom;
                        if (item.GridLocation.Y > p1.Y) p1.Y = item.GridLocation.Y;
                    }
                    if (farthestPoint(3, item.Bounds.Location) == item.Bounds.Location)
                    {
                        g.DrawLine(outline, item.Bounds.X, item.Bounds.Y + item.Bounds.Height, item.Bounds.X, item.Bounds.Y - widthBufY);
                        item.OutlineDirs.Add(Gridcell.OutlineDirection.West);
                    }
                }
            }
            canvasSize = new Size((p1.X + cellSize) * cellSize + 5, (p1.Y + cellSize) * cellSize + 5);
            //canvasSize = new Size(500,500);
        }

        void drawOnGrid()
        {
            Pen rec = new Pen(Color.Gray);
            Pen outline = new Pen(Color.White, 1);

            using (var g = Graphics.FromImage(overlay))
            {
                g.Clear(Color.Transparent);
                foreach (var item in board.Cells)
                {
                    // Vorhandene Wände
                    if (item.Lines[0].Set)
                    {
                        g.DrawLine(outline, item.Bounds.Right, item.Bounds.Top, item.Bounds.Right, item.Bounds.Bottom);
                    }
                    if (item.Lines[1].Set)
                    {
                        g.DrawLine(outline, item.Bounds.Left, item.Bounds.Bottom, item.Bounds.Right, item.Bounds.Bottom);
                    }
                    if (item.Taken)
                    {
                        g.DrawImage(symbols[player_1.Symbol], item.Bounds);
                    }
                }
            }
            canvas.Invalidate();
        }

        async void DoDraw()
        {
            Size s = new Size(board.Cells.Max(x => x.GridLocation.X) * cellSize + cellSize * 2, board.Cells.Max(x => x.GridLocation.Y) * cellSize + cellSize * 2);
            canvas.Size = s;
            screen = new Bitmap(s.Width, s.Height);
            overlay = new Bitmap(s.Width, s.Height);
            await Task.Run(() => draw());
        }

        async void DoOnGridDraw()
        {
            await Task.Run(() => drawOnGrid());
        }

        void generateGrid()
        {
            int[,] grid = maps.getRandomMap();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i,j] == 1)
                    {
                        board.Cells.Add(new Gridcell() { GridLocation = new Point(j, i), Bounds = new Rectangle(pZero.X + j * cellSize, pZero.Y + i * cellSize, cellSize, cellSize) });
                    }
                }
            }
        }

        void generateGrid(int index)
        {
            int[,] grid = maps.getMap(index);
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 1)
                    {
                        board.Cells.Add(new Gridcell() { GridLocation = new Point(j, i), Bounds = new Rectangle(pZero.X + j * cellSize, pZero.Y + i * cellSize, cellSize, cellSize) });
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            


            if (Screen.AllScreens.Length <= 1)
            {
                Location = new Point(200,200);
            }
            this.DoubleBuffered = true;
            player_1.Color = Color.DarkOrange;
            Random r = new Random();
            player_1.Name = "New_Player " + r.Next(1,100);
            lbl_playerName.Text = player_1.Name;
            btn_playerColor.BackColor = player_1.Color;
            //Discoverer.PeerJoined = ip => Console.WriteLine("JOINED:" + ip);
            //Discoverer.PeerLeft = ip => Console.WriteLine("LEFT:" + ip);

            //Discoverer.Start();

            generateGrid();
            //foreach (var item in board.Cells)
            //{
            //    try
            //    {
            //        var gc = board.Cells.Find(a => a.Bounds.Y == item.Bounds.Y - item.Bounds.Height && a.Bounds.X == item.Bounds.X);
            //        item.UpperCell = gc;
            //    }
            //    catch (Exception) {  }
            //    try
            //    {
            //        var gc = board.Cells.Find(a => a.Bounds.X == item.Bounds.X - item.Bounds.Width && a.Bounds.Y == item.Bounds.Y);
            //        item.LeftCell = gc;
            //    }
            //    catch (Exception) {  }
            //}


            screen = new Bitmap(canvas.Width, canvas.Height);
            overlay = new Bitmap(canvas.Width, canvas.Height);
            DoDraw();
        }

        private void button_triggerLeft_Click(object sender, EventArgs e)
        {
            if (panel_left.Location.X >= 0)
            {
                panel_left.Location = new Point(0 - panel_left.Width, panel_left.Location.Y);
            }
            else
            {
                panel_left.Location = new Point(0, panel_left.Location.Y);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panel_left.Height = this.Height - panel1.Height;
            canvas.Invalidate();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(screen, 0, 0);
            e.Graphics.DrawImage(overlay, 0, 0);
            Gridcell g = getCellOnCursor(MPos);

            GraphicsPath p1 = new GraphicsPath();
            GraphicsPath p2 = new GraphicsPath();
            GraphicsPath p3 = new GraphicsPath();
            GraphicsPath p4 = new GraphicsPath();
            

            if (g != null)
            { 

                p1.AddPolygon(new Point[] { new Point(g.Bounds.Location.X, g.Bounds.Location.Y), new Point(g.Bounds.X + g.Bounds.Width, g.Bounds.Y), new Point(g.Bounds.Location.X + g.Bounds.Width / 2, g.Bounds.Location.Y + g.Bounds.Height / 2), new Point(g.Bounds.Location.X, g.Bounds.Location.Y) });
                p2.AddPolygon(new Point[] { new Point(g.Bounds.Location.X + g.Bounds.Width, g.Bounds.Location.Y), new Point(g.Bounds.X + g.Bounds.Width, g.Bounds.Y + g.Bounds.Height), new Point(g.Bounds.Location.X + g.Bounds.Width / 2, g.Bounds.Location.Y + g.Bounds.Height / 2), new Point(g.Bounds.Location.X + g.Bounds.Width, g.Bounds.Location.Y) });
                p3.AddPolygon(new Point[] { new Point(g.Bounds.Location.X, g.Bounds.Location.Y + g.Bounds.Height), new Point(g.Bounds.X + g.Bounds.Width, g.Bounds.Y + g.Bounds.Height), new Point(g.Bounds.Location.X + g.Bounds.Width / 2, g.Bounds.Location.Y + g.Bounds.Height / 2), new Point(g.Bounds.Location.X, g.Bounds.Location.Y + g.Bounds.Height) });
                p4.AddPolygon(new Point[] { new Point(g.Bounds.Location.X, g.Bounds.Location.Y), new Point(g.Bounds.X, g.Bounds.Y + g.Bounds.Height), new Point(g.Bounds.Location.X + g.Bounds.Width / 2, g.Bounds.Location.Y + g.Bounds.Height / 2), new Point(g.Bounds.Location.X, g.Bounds.Location.Y) });


                Point[] line = new Point[2];
                if (p1.IsVisible(MPos))
                {
                    if (!g.OutlineDirs.Contains(Gridcell.OutlineDirection.North))
                    {
                        line = new Point[] { new Point(g.Bounds.X, g.Bounds.Y), new Point(g.Bounds.X + g.Bounds.Width, g.Bounds.Y) };
                    }
                }
                else if(p2.IsVisible(MPos))
                {
                    if (!g.OutlineDirs.Contains(Gridcell.OutlineDirection.East))
                    {
                        line = new Point[] { new Point(g.Bounds.X + g.Bounds.Width, g.Bounds.Y), new Point(g.Bounds.X + g.Bounds.Width, g.Bounds.Y + g.Bounds.Height) };
                    }
                }
                else if (p3.IsVisible(MPos))
                {
                    if (!g.OutlineDirs.Contains(Gridcell.OutlineDirection.South))
                    {
                        line = new Point[] { new Point(g.Bounds.X, g.Bounds.Y + g.Bounds.Height), new Point(g.Bounds.X + g.Bounds.Width, g.Bounds.Y + g.Bounds.Height) };
                    }
                }
                else if (p4.IsVisible(MPos))
                {
                    if (!g.OutlineDirs.Contains(Gridcell.OutlineDirection.West))
                    {
                        line = new Point[] { new Point(g.Bounds.X, g.Bounds.Y), new Point(g.Bounds.X, g.Bounds.Y + g.Bounds.Height) };
                    }
                }
                e.Graphics.DrawLine(new Pen(player_1.Color, 2), line[0], line[1]);
                //if (g.UpperCell != null && g.UpperCell.Lines[0].Set) e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Red)), g.UpperCell.Bounds);
                //if (g.LeftCell != null) e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Green)), g.LeftCell.Bounds);
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            MPos = e.Location;
            canvas.Invalidate();
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            var g = getCellOnCursor(MPos);

            GraphicsPath p1 = new GraphicsPath();
            GraphicsPath p2 = new GraphicsPath();
            GraphicsPath p3 = new GraphicsPath();
            GraphicsPath p4 = new GraphicsPath();

            if (g != null)
            {
                p1.AddPolygon(new Point[] { new Point(g.Bounds.Location.X, g.Bounds.Location.Y), new Point(g.Bounds.X + g.Bounds.Width, g.Bounds.Y), new Point(g.Bounds.Location.X + g.Bounds.Width / 2, g.Bounds.Location.Y + g.Bounds.Height / 2), new Point(g.Bounds.Location.X, g.Bounds.Location.Y) });
                p2.AddPolygon(new Point[] { new Point(g.Bounds.Location.X + g.Bounds.Width, g.Bounds.Location.Y), new Point(g.Bounds.X + g.Bounds.Width, g.Bounds.Y + g.Bounds.Height), new Point(g.Bounds.Location.X + g.Bounds.Width / 2, g.Bounds.Location.Y + g.Bounds.Height / 2), new Point(g.Bounds.Location.X + g.Bounds.Width, g.Bounds.Location.Y) });
                p3.AddPolygon(new Point[] { new Point(g.Bounds.Location.X, g.Bounds.Location.Y + g.Bounds.Height), new Point(g.Bounds.X + g.Bounds.Width, g.Bounds.Y + g.Bounds.Height), new Point(g.Bounds.Location.X + g.Bounds.Width / 2, g.Bounds.Location.Y + g.Bounds.Height / 2), new Point(g.Bounds.Location.X, g.Bounds.Location.Y + g.Bounds.Height) });
                p4.AddPolygon(new Point[] { new Point(g.Bounds.Location.X, g.Bounds.Location.Y), new Point(g.Bounds.X, g.Bounds.Y + g.Bounds.Height), new Point(g.Bounds.Location.X + g.Bounds.Width / 2, g.Bounds.Location.Y + g.Bounds.Height / 2), new Point(g.Bounds.Location.X, g.Bounds.Location.Y) });


                Point[] line = new Point[2];
                if (p1.IsVisible(MPos))
                {
                    if (!g.OutlineDirs.Contains(Gridcell.OutlineDirection.North))
                    {
                        try
                        {
                            upperCell(g).Lines[1].Set = true;
                            //var gc = board.Cells.Find(a => a.Bounds.Y == g.Bounds.Y - g.Bounds.Height && a.Bounds.X == g.Bounds.X);
                            //gc.Lines[1].Set = true;
                        }
                        catch (Exception) { }
                    }
                }
                else if (p2.IsVisible(MPos))
                {
                    if (!g.OutlineDirs.Contains(Gridcell.OutlineDirection.East))
                    {
                        try
                        {
                            g.Lines[0].Set = true;
                        }
                        catch (Exception) { }
                    }
                }
                else if (p3.IsVisible(MPos))
                {
                    if (!g.OutlineDirs.Contains(Gridcell.OutlineDirection.South))
                    {
                        try
                        {
                            g.Lines[1].Set = true;
                        }
                        catch (Exception) { }
                    }
                }
                else if (p4.IsVisible(MPos))
                {
                    if (!g.OutlineDirs.Contains(Gridcell.OutlineDirection.West))
                    {
                        try
                        {
                            leftCell(g).Lines[0].Set = true;
                            Console.WriteLine("Line 0" + leftCell(g).Lines[0].Set);
                            //var gc = board.Cells.Find(a => a.Bounds.X == g.Bounds.X - g.Bounds.Width && a.Bounds.Y == g.Bounds.Y);
                            //gc.Lines[0].Set = true;
                        }
                        catch (Exception) { }
                    }
                }
                CheckOnAvailable();
            }
        }

        async void CheckOnAvailable()
        {
            bool possible = await Task.Run(() => isCheckPossible());
            while (possible)
            {
                possible = await Task.Run(() => isCheckPossible());
                await Task.Run(() => applyChecks());
                applyChecks();
            }
            DoOnGridDraw();
        }

        

        void receiveThread()
        {
            while (true)
            {
                TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();

                Console.WriteLine("Waiting for connection...");

                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                Console.WriteLine("Connected with {0}", tcpClient.Client.RemoteEndPoint);

                while (!(tcpClient.Client.Poll(20, SelectMode.SelectRead)))
                {
                    NetworkStream networkStream = tcpClient.GetStream();
                    StreamReader streamReader = new StreamReader(networkStream);

                    string data = streamReader.ReadLine();

                    if (data != null)
                    {
                        Console.WriteLine("Received data: {0}", data);
                    }
                }
                Console.WriteLine("Dissconnected...\n");
                tcpListener.Stop();
            }
        }


        private async void btn_host_Click(object sender, EventArgs e)
        {
            HostNewGame host = new HostNewGame();
            if (host.ShowDialog() == DialogResult.OK)
            {
                LobbyForm f = new LobbyForm(host.Servername, true, player_1);
                f.Show();
            }
        }

        private void btn_join_Click(object sender, EventArgs e)
        {
            JoinGame join = new JoinGame(player_1);
            join.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_playerColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                player_1.Color = colorDialog1.Color;
                btn_playerColor.BackColor = player_1.Color;
            }
        }

        private void btn_symbol_Click(object sender, EventArgs e)
        {


            btn_symbol.Invalidate();
        }

        private void btn_symbol_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(symbols[player_1.Symbol], btn_symbol.Bounds);
        }
    }
}
