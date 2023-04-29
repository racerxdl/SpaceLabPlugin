using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceLabAPI.Extensions
{
    public static class ExtendedVoxel
    {
        public static double GetSize(this MyVoxelBase voxel)
        {
            if (voxel.DebugName.IndexOf("MyPlanet") >= 0)
            {
                MyPlanet p = voxel as MyPlanet;

                return p.AverageRadius * 2;
            }

            return Math.Max(Math.Max(voxel.Size.X, voxel.Size.Y), voxel.Size.Z) / 2.5;
        }

        public static double GetAtmosphereAlt(this MyVoxelBase voxel)
        {
            if (voxel.DebugName.IndexOf("MyPlanet") >= 0)
            {
                MyPlanet p = voxel as MyPlanet;

                return p.AtmosphereAltitude;
            }

            return 0;
        }

        public static bool HasAtmosphere(this MyVoxelBase voxel)
        {
            if (voxel.DebugName.IndexOf("MyPlanet") >= 0)
            {
                MyPlanet p = voxel as MyPlanet;
                return p.HasAtmosphere;
            }
            return false;
        }

        public static Tuple<double, double> GetHillParams(this MyVoxelBase voxel)
        {
            if (voxel.DebugName.IndexOf("MyPlanet") >= 0)
            {
                MyPlanet p = voxel as MyPlanet;
                return new Tuple<double, double>(p.Generator.HillParams.Min, p.Generator.HillParams.Max);
            }

            return new Tuple<double, double>(0, 0);
        }
    }
}
