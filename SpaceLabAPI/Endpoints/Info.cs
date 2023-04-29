using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using SharpBoss.Attributes;
using SharpBoss.Attributes.Methods;
using SpaceLabAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Groups;
using VRageMath;
using SpaceLabAPI.Extensions;

namespace SpaceLabAPI.Endpoints
{

    [REST("/info")]
    public class Info
    {
        [GET("/global")]
        public GlobalInfo GlobalInfo()
        {
            return  new GlobalInfo { 
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
            return MySession.Static.VoxelMaps.Instances.GetVoxels();
        }

        [GET("/factions")]
        public List<Faction> GetFactions()
        {
            return MySession.Static.Factions.GetFactions();
        }

        [GET("/players")]
        public List<Player> GetPlayers()
        {
            return MySession.Static.Players.GetIdentity();
        }

        [GET("/v2grids")]
        public List<GridGroup> GetV2Grids()
        {
            List<GridGroup> groups = new List<GridGroup>();


            foreach (var group in MyCubeGridGroups.Static.Physical.Groups.ToList())
            {
                GridGroup ggroup = new GridGroup();
                Dictionary<string, int> owners = new Dictionary<string, int>();
                Dictionary<string, string> factions = new Dictionary<string, string>();
                Dictionary<string, string> factionTags = new Dictionary<string, string>();

                owners["NONE"] = 0;
                factionTags["NONE"] = "NONE";
                factions["NONE"] = "NONE";
                ggroup.Grids = new List<Grid>();


                foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node groupNodes in group.Nodes)
                {
                    MyCubeGrid grid = groupNodes.NodeData;
                    var ownerName = grid.GetOwner();
                    var factionName = grid.GetFactionName();
                    var factionTag = grid.GetFactionTag();

                    factions[ownerName] = factionName;
                    factionTags[ownerName] = factionTag;

                    if (!owners.ContainsKey(ownerName))
                    {
                        owners[ownerName] = 0;
                    }

                    owners[ownerName]++;

                    ggroup.Grids.Add(new Grid
                    {
                        Id = grid.EntityId.ToString(),
                        Name = grid.DisplayName,
                        Owner = ownerName,
                        Position = grid.WorldMatrix.Translation,
                        Faction = factionName,
                        FactionTag = factionTag,
                        Blocks = grid.BlocksCount,
                        IsPowered = grid.IsPowered,
                        HasAntena = grid.HasAntena(false),
                        AntenaIsWorking = grid.HasAntena(true),
                        IsStatic = grid.IsStatic,
                        IsParked = grid.IsParked,
                        GridSize = grid.GridSize,
                        PCU = grid.BlocksPCU,
                        ParentId = grid.Parent?.EntityId.ToString(),
                        RelGroupId = groups.Count,
                        RelGroupCount = group.Nodes.Count,
                    });
                    ggroup.Blocks += grid.BlocksCount;
                }

                ggroup.Owner = owners.Aggregate((a, b) =>
                {
                    return a.Value > b.Value ? a : b;
                }).Key;
                ggroup.Faction = factions.GetValueOrDefault(ggroup.Owner);
                ggroup.Tag = factionTags.GetValueOrDefault(ggroup.Owner);

                groups.Add(ggroup);
            }
            return groups;
        }

        [GET("/grids")]
        public List<Grid> GetGrids()
        {
            List<Grid> grids = new List<Grid>();
            int relGroupId = 0;
            foreach (var group in MyCubeGridGroups.Static.Physical.Groups.ToList())
            {
                foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node groupNodes in group.Nodes.ToList())
                {
                    MyCubeGrid grid = groupNodes.NodeData;
                    var ownerId = grid.BigOwners.Count > 0 ? grid.BigOwners[0] : -1;
                    var ownerName = "NONE";
                    var factionName = "NONE";
                    var factionTag = "NONE";
                    
                    if (ownerId != -1)
                    {
                        ownerName = MySession.Static.Players.TryGetIdentity(ownerId)?.DisplayName ?? "NONE";
                        factionName = MySession.Static.Factions.TryGetPlayerFaction(ownerId)?.Name ?? "NONE";
                        factionTag = MySession.Static.Factions.TryGetPlayerFaction(ownerId)?.Tag ?? "NONE";
                    }
                    grids.Add(new Grid
                    {
                        Id = grid.EntityId.ToString(),
                        Name = grid.DisplayName,
                        Owner = ownerName,
                        Position = grid.WorldMatrix.Translation,
                        Faction = factionName,
                        FactionTag = factionTag,
                        Blocks = grid.BlocksCount,
                        IsPowered = grid.IsPowered,
                        IsStatic = grid.IsStatic,
                        IsParked = grid.IsParked,
                        GridSize = grid.GridSize,
                        PCU = grid.BlocksPCU,
                        ParentId = grid.Parent?.EntityId.ToString(),
                        RelGroupId = relGroupId,
                        RelGroupCount = group.Nodes.Count,
                    });
                }
                relGroupId++;
            }
            return grids;
        }
    }
}
