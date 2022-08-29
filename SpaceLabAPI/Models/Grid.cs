using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace SpaceLabAPI.Models
{
    public class Grid
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Faction { get; set; }
        public string FactionTag { get; set; }
        public int Blocks { get; set; }
        public bool IsPowered { get; set; }
        public double GridSize { get; set; }
        public bool IsStatic { get; set; }
        public bool IsParked { get; set; }
        public string ParentId { get; set; }
        public int RelGroupId { get; set; }
        public int RelGroupCount { get; set; }
        public int PCU { get; set; }
        public Vector3D Position { get; set; }
        public double X { get { return Position.X; } }
        public double Y { get { return Position.Y; } }
        public double Z { get { return Position.Z; } }
    }
}
