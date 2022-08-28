using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace SpaceLabAPI.Models
{
    public class Voxel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DebugName { get; set; }
        public Vector3D Position { get; set; }
        public Vector3 Size { get; set; }
        public double X { get { return Position.X; } }
        public double Y { get { return Position.Y; } }
        public double Z { get { return Position.Z; } }
        public double SizeX { get { return Size.X / 2.5f; } }
        public double SizeY { get { return Size.Y / 2.5f; } }
        public double SizeZ { get { return Size.Z / 2.5f; } }
    }
}
