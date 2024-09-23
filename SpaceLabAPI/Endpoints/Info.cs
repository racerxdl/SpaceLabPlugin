using Sandbox.Game.Entities;
using Sandbox.Game.World;
using SharpBoss.Attributes;
using SharpBoss.Attributes.Methods;
using SpaceLabAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SpaceLab;
using SpaceLabAPI.Extensions;
using SpaceLab.Models;

namespace SpaceLabAPI.Endpoints
{
    [REST("/info")]
    public class Info
    {
        [GET("/global")]
        public GlobalInfo GlobalInfo()
        {
            return new GlobalInfo
            {
                SunNormalizedPosition = MySector.DirectionToSunNormalized,
                LargeShipMaxAngularSpeed = MySector.EnvironmentDefinition?.LargeShipMaxAngularSpeed,
                LargeShipMaxSpeed = MySector.EnvironmentDefinition?.LargeShipMaxSpeed,
                SmallShipMaxAngularSpeed = MySector.EnvironmentDefinition?.SmallShipMaxAngularSpeed,
                SmallShipMaxSpeed = MySector.EnvironmentDefinition?.SmallShipMaxSpeed,
                SunIntensity = MySector.EnvironmentDefinition?.SunProperties.SunIntensity
            };
        }

        [GET("/test")]
        public string Test()
        {
            string result = "";
            result += "Server Info: <BR/>";
            var sun = MySector.DirectionToSunNormalized;
            result += $"Sun: {sun.X} {sun.Y} {sun.Z}<BR/>";
            result += $"Sun Intensity: {MySector.EnvironmentDefinition?.SunProperties.SunIntensity}<BR/>";
            result += $"Small Ship Max Speed: {MySector.EnvironmentDefinition?.SmallShipMaxSpeed}<BR/>";
            result += $"Small Ship Max Angular Speed: {MySector.EnvironmentDefinition?.SmallShipMaxAngularSpeed}<BR/>";
            result += $"Large Ship Max Speed: {MySector.EnvironmentDefinition?.LargeShipMaxSpeed}<BR/>";
            result += $"Large Ship Max Angular Speed: {MySector.EnvironmentDefinition?.LargeShipMaxAngularSpeed}<BR/>";
            result += "<BR/>";

            MySession.Static.VoxelMaps.Instances.ToList().ForEach((v) =>
            {
                result += "<BR/>";
                result += $"Voxel: {v.DebugName}<BR/>";
                result += $"Size: {v.Size}<BR/>";
                result += $"SizeInMeters: {v.SizeInMetres}<BR/>";
                result += $"SizeInMetersHalf: {v.SizeInMetresHalf}<BR/>";
                result += $"SizeMinusOne: {v.SizeMinusOne}<BR/>";
                result += $"VoxelSize: {v.VoxelSize}<BR/>";
                result += $"Scale: {v.WorldMatrix.Scale}<BR/>";
                result += $"ScaleGroup: {v.ScaleGroup}<BR/>";
                result += $"BoundingBoxSize: {v.Model?.BoundingBoxSize}<BR/>";
                result += $"RootVoxel: {v.RootVoxel?.ToString()}<BR/>";
                result += $"RootVoxelSize: {v.RootVoxel?.Size}<BR/>";
                result += $"RootVoxelSize: {v.RootVoxel?.SizeInMetres}<BR/>";
                result += $"RootVoxelRootVoxel: {v.RootVoxel?.RootVoxel?.ToString()}<BR/>";
                result += $"RootVoxelRootVoxelSize: {v.RootVoxel?.RootVoxel?.Size}<BR/>";
                result += $"RootVoxelRootVoxelSize: {v.RootVoxel?.RootVoxel?.SizeInMetres}<BR/>";
                if (v.DebugName.IndexOf("MyPlanet") >= 0)
                {
                    MyPlanet p = v as MyPlanet;

                    result += $"Minimum Radius: {p?.MinimumRadius}<BR/>";
                    result += $"Average Radius: {p?.AverageRadius}<BR/>";
                    result += $"Maximum Radius: {p?.MaximumRadius}<BR/>";
                    result += $"HillParams: {p?.Generator?.HillParams.Min};{p?.Generator?.HillParams.Max}<BR/>";
                }

                result += "<BR/><BR/>";
            });
            return result.Replace("<BR/>", Environment.NewLine);
        }


        [GET("/voxels")]
        public List<Voxel> GetVoxels()
        {
            return SpaceLabServer.Store.Voxels;
        }

        [GET("/factions")]
        public List<Faction> GetFactions()
        {
            return MySession.Static.Factions.GetFactions();
        }

        [GET("/players")]
        public List<Player> GetPlayers() => SpaceLabServer.Store.Players.Values.ToList();

        [GET("/grids")]
        public List<Grid> GetGrids() => SpaceLabServer.Store.Grids.Values.ToList();
    }
}