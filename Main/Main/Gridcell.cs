using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class Gridcell
    {
        public Gridcell()
        {
            lines[0] = new line() { Set = false, SetBy = "" };
            lines[1] = new line() { Set = false, SetBy = "" };
        }

        public OutlineDirection readyToFill()
        {
            int[] vals = new int[4];
            if (upperCell.lines[1].Set) vals[0] = 1;
            if (lines[0].Set) vals[1] = 1;
            if (lines[1].Set) vals[2] = 1;
            if (leftCell.lines[0].Set) vals[3] = 1;

            if (true)
            {

            }

            var count = vals.Count(i => i > 0);
            Console.WriteLine("Count "+count);
            return OutlineDirection.None;
        }

        public struct line
        {
            bool set;
            string setBy;

            public string SetBy { get => setBy; set => setBy = value; }
            public bool Set { get => set; set => set = value; }
        }

        public enum OutlineDirection
        {
            None,
            North,
            East,
            South,
            West
        }
        List<OutlineDirection> outlineDirs = new List<OutlineDirection>();
        Rectangle bounds = new Rectangle();
        line[] lines = new line[2];
        bool taken = false;
        Guid playerId;
        Gridcell upperCell = null;
        Gridcell leftCell = null;
        
        public bool Taken { get => taken; set => taken = value; }
        public Guid PlayerId { get => playerId; set => playerId = value; }
        public Rectangle Bounds { get => bounds; set => bounds = value; }
        public line[] Lines { get => lines; set => lines = value; }
        public List<OutlineDirection> OutlineDirs { get => outlineDirs; set => outlineDirs = value; }
        public Gridcell UpperCell { get => upperCell; set => upperCell = value; }
        public Gridcell LeftCell { get => leftCell; set => leftCell = value; }
    }
}
