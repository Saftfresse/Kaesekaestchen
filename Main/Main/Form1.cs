using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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

        Size canvasSize = new Size();
        Point MPos = new Point();
        int cellSize = 40;
        Point pZero = new Point(5,5);
        
        Gridcell getCellOnCursor()
        {
            Gridcell g = null;
            foreach (var item in board.Cells)
            {
                if (item.Bounds.Contains(MPos))
                {
                    g = item;
                }
            }
            return g;
        }

        Point farthestPoint(int dir, Point loc)
        {
            Point p = new Point(0,0);
            switch (dir)
            {
                case 0:
                    p = new Point(canvas.Width, canvas.Height);
                    foreach (var item in board.Cells)
                    {
                        if (item.Bounds.Location.X == loc.X && item.Bounds.Location.Y < p.Y)
                        {
                            p = item.Bounds.Location;
                        }
                    }
                    break;
                case 1:
                    p = new Point(0, 0);
                    foreach (var item in board.Cells)
                    {
                        if (item.Bounds.Location.Y == loc.Y && item.Bounds.Location.X > p.X)
                        {
                            p = item.Bounds.Location;
                        }
                    }
                    break;
                case 2:
                    p = new Point(0, 0);
                    foreach (var item in board.Cells)
                    {
                        if (item.Bounds.Location.X == loc.X && item.Bounds.Location.Y > p.Y)
                        {
                            p = item.Bounds.Location;
                        }
                    }
                    break;
                case 3:
                    p = new Point(canvas.Width, canvas.Height);
                    foreach (var item in board.Cells)
                    {
                        if (item.Bounds.Location.Y == loc.Y && item.Bounds.Location.X < p.X)
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
                try
                {
                    var gc = board.Cells.Find(a => a.Bounds.Y == item.Bounds.Y - item.Bounds.Height && a.Bounds.X == item.Bounds.X);
                    if (gc.Lines[1].Set)
                    {
                        count++;
                    }
                } catch (Exception) { }
                try
                {
                    var gc = board.Cells.Find(a => a.Bounds.X == item.Bounds.X - item.Bounds.Width && a.Bounds.Y == item.Bounds.Y);
                    if (gc.Lines[0].Set)
                    {
                        count++;
                    }
                }
                catch (Exception) { }
                if (item.Lines[0].Set) count++;
                if (item.Lines[1].Set) count++;
            }



            return ret;
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
            using (var g = Graphics.FromImage(screen))
            {
                g.Clear(Color.FromArgb(19, 19, 19));
                foreach (var item in board.Cells)
                {
                    g.DrawRectangle(rec, item.Bounds);

                    // Ränder
                    Console.WriteLine(farthestPoint(3, item.Bounds.Location));
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
                    }
                    if (farthestPoint(2, item.Bounds.Location) == item.Bounds.Location)
                    {
                        g.DrawLine(outline, item.Bounds.X + item.Bounds.Width, item.Bounds.Y + item.Bounds.Height, item.Bounds.X - widthBufY, item.Bounds.Y + item.Bounds.Height);
                        item.OutlineDirs.Add(Gridcell.OutlineDirection.South);
                        bottom = bottom < item.Bounds.Y + item.Bounds.Height ? item.Bounds.Y + item.Bounds.Height : bottom;
                    }
                    if (farthestPoint(3, item.Bounds.Location) == item.Bounds.Location)
                    {
                        g.DrawLine(outline, item.Bounds.X, item.Bounds.Y + item.Bounds.Height, item.Bounds.X, item.Bounds.Y - widthBufY);
                        item.OutlineDirs.Add(Gridcell.OutlineDirection.West);
                    }
                }
            }
            canvasSize = new Size(left + 5, bottom + 5);
        }

        void drawOnGrid()
        {
            Pen rec = new Pen(Color.Gray);
            Pen outline = new Pen(Color.WhiteSmoke, 3);
            using (var g = Graphics.FromImage(screen))
            {
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
                }
            }
        }

        async void DoDraw()
        {
            await Task.Run(() => draw());
            canvas.Size = canvasSize;
        }

        async void DoOnGridDraw()
        {
            await Task.Run(() => drawOnGrid());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            //Discoverer.PeerJoined = ip => Console.WriteLine("JOINED:" + ip);
            //Discoverer.PeerLeft = ip => Console.WriteLine("LEFT:" + ip);

            //Discoverer.Start();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    board.Cells.Add(new Gridcell() { Bounds = new Rectangle(pZero.X + j * cellSize, pZero.Y + i * cellSize, cellSize, cellSize) });
                }
            }
            
            screen = new Bitmap(canvas.Width, canvas.Height);
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
            panel_left.Height = canvas.Height;
            screen = new Bitmap(canvas.Width, canvas.Height);
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(screen, 0, 0);
            Gridcell g = getCellOnCursor();

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
                e.Graphics.DrawLine(new Pen(Color.DarkOrange, 3), line[0], line[1]);
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            MPos = e.Location;
            canvas.Invalidate();
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            var g = getCellOnCursor();

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
                            var gc = board.Cells.Find(a => a.Bounds.Y == g.Bounds.Y - g.Bounds.Height && a.Bounds.X == g.Bounds.X);
                            gc.Lines[1].Set = true;
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
                            var gc = board.Cells.Find(a => a.Bounds.X == g.Bounds.X - g.Bounds.Width && a.Bounds.Y == g.Bounds.Y);
                            gc.Lines[0].Set = true;
                        }
                        catch (Exception) { }
                    }
                }
                DoOnGridDraw();
            }
        }
    }
}
