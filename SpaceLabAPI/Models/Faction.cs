using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceLabAPI.Models
{
    public class Faction
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string PublicNote { get; set; }
        public string FounderName { get; set; }
        public string[] Members { get; set; }
    }
}
