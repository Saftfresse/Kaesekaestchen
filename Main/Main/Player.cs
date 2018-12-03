using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class Player
    {
        public Player()
        { }

        Guid uid = Guid.NewGuid();
        string name = "New Player";
        char symbol = 'x';

        public Guid Uid { get => uid; set => uid = value; }
        public string Name { get => name; set => name = value; }
        public char Symbol { get => symbol; set => symbol = value; }
    }
}
