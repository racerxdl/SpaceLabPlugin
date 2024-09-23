using System;
using VRageMath;

namespace SpaceLab.Models
{
    public class Voxel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DebugName { get; set; }
        public Vector3D Position { get; set; }
        public Quaternion Rotation { get; set; }
        public double Size { get; set; }
        public bool HasAtmosphere { get; set; }
        public double AtmosphereAltitude { get; set; }
        public double X { get { return Position.X; } }
        public double Y { get { return Position.Y; } }
        public double Z { get { return Position.Z; } }

        public Tuple<double, double> HillParameters { get; set; }
    }
}
