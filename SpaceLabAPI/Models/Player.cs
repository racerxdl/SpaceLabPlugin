using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace SpaceLabAPI.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string SteamId { get; set; }
        public bool IsOnline { get; set; }
        public Vector3D Position { get; set; }
        public double X { get { return Position.X; } }
        public double Y { get { return Position.Y; } }
        public double Z { get { return Position.Z; } }
    }
}
