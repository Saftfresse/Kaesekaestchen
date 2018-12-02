using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class Gridboard
    {
        public Gridboard()
        {

        }

        List<Gridcell> cells = new List<Gridcell>();

        public List<Gridcell> Cells { get => cells; set => cells = value; }
    }
}
