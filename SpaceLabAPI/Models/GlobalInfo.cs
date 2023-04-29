using Sandbox.Game.World;
using SharpBoss.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace SpaceLabAPI.Models
{
    public class GlobalInfo
    {
        public Vector3D SunNormalizedPosition { get; set; }
        public float? SunIntensity { get; set; }
        public double SunNormalizedX { get { return SunNormalizedPosition.X; } }
        public double SunNormalizedY { get { return SunNormalizedPosition.Y; } }
        public double SunNormalizedZ { get { return SunNormalizedPosition.Z; } }

        public float? SmallShipMaxSpeed { get; set; }
        public float? SmallShipMaxAngularSpeed { get; set; }
        public float? LargeShipMaxSpeed { get; set; }
        public float? LargeShipMaxAngularSpeed { get; set; }
    }
}
