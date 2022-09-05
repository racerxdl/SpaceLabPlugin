﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace SpaceLabAPI.Models
{
    public class Voxel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DebugName { get; set; }
        public Vector3D Position { get; set; }
        public double Size { get; set; }
        public bool HasAtmosphere { get; set; }
        public double AtmosphereAltitude { get; set; }
        public double X { get { return Position.X; } }
        public double Y { get { return Position.Y; } }
        public double Z { get { return Position.Z; } }
        // Deprecated
        public double SizeX { get { return Size; } }
        // Deprecated
        public double SizeY { get { return Size; } }
        // Deprecated
        public double SizeZ { get { return Size; } }
    }
}
