using System.Collections.Generic;

namespace SpaceLab.Models
{
    public class GridGroup
    {
        public List<Grid> Grids { get; set; } = new List<Grid>();
        public string Owner { get; set; }
        public string Faction { get; set; }
        public string Tag { get; set; }
        public int Blocks { get; set; }

        public Dictionary<string, int> Owners = new Dictionary<string, int>();
        public Dictionary<string, string> Factions = new Dictionary<string, string>();
        public Dictionary<string, string> FactionTags = new Dictionary<string, string>();
    }
}