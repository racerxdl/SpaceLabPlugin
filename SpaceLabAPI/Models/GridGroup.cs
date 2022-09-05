using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceLabAPI.Models
{
    public class GridGroup
    {
        public List<Grid> Grids { get; set; }
        public string Owner { get; set; }
        public string Faction { get; set; }
        public string Tag { get; set; }
        public int Blocks { get; set; }
    }
}
