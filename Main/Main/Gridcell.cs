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
        string playerId = "";
        
        public bool Taken { get => taken; set => taken = value; }
        public string PlayerId { get => playerId; set => playerId = value; }
        public Rectangle Bounds { get => bounds; set => bounds = value; }
        public line[] Lines { get => lines; set => lines = value; }
        public List<OutlineDirection> OutlineDirs { get => outlineDirs; set => outlineDirs = value; }
    }
}
