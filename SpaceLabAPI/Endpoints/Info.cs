using Sandbox.Game.Entities;
using Sandbox.Game.World;
using SharpBoss.Attributes;
using SharpBoss.Attributes.Methods;
using SharpBoss.Logging;
using SpaceLabAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Groups;
using VRageMath;

namespace SpaceLabAPI.Endpoints
{

    [REST("/info")]
    public class Info
    {
        [GET("/voxels")]
        public List<Voxel> GetVoxels()
        {
            return MySession.Static.VoxelMaps.Instances.ToList().Select((v) => new Voxel
            {
                Id = v.EntityId,
                Name = v.Name,
                DebugName = v.DebugName,
                Position = v.WorldMatrix.Translation,
                Size = v.Size,
            }).ToList();
        }

        [GET("/factions")]
        public List<Faction> GetFactions()
        {
            return MySession.Static.Factions.GetAllFactions().Select((f) => new Faction
            {
                Name = f.Name,
                Tag = f.Tag,
                PublicNote = f.Description,
                FounderName = MySession.Static.Players.TryGetIdentity(f.FounderId)?.DisplayName ?? "Unavailable",
                Members = f.Members
                .Select((kv) => kv.Value)
                .Select((v) => v.PlayerId)
                .Select((id) => MySession.Static.Players.TryGetIdentity(id))
                .Where((p) => p != null)
                .Select((p) => p.DisplayName).ToArray()
            }).ToList();
        }

        [GET("/players")]
        public List<Player> GetPlayers()
        {
            var allPlayerIdentities = MySession.Static.Players.GetAllIdentities();
            var onlinePlayers = MySession.Static.Players.GetOnlinePlayers().Select((p) => p.Identity.IdentityId).ToList();
            var playerDict = new Dictionary<long, MyPlayer>();
            MySession.Static.Players.GetOnlinePlayers().ForEach((p) => { playerDict[p.Identity.IdentityId] = p; });
            return allPlayerIdentities.Where((p) => p != null).Select((p) => new Player
            {
                Id = p.IdentityId,
                SteamId = playerDict.GetValueOrDefault(p.IdentityId)?.Id.SteamId ?? 0,
                Name = p.DisplayName,
                Faction = MySession.Static.Factions.TryGetPlayerFaction(p.IdentityId)?.Name ?? "NONE",
                IsOnline = onlinePlayers.Contains(p.IdentityId),
                Position = playerDict.GetValueOrDefault(p.IdentityId)?.GetPosition() ?? new Vector3D()
            }).ToList();
        }

        [GET("/grids")]
        public List<Grid> GetGrids()
        {
            List<Grid> grids = new List<Grid>();
            foreach (var group in MyCubeGridGroups.Static.Physical.Groups.ToList())
            {
                foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node groupNodes in group.Nodes)
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
                        Blocks = grid.BlocksCount
                    });
                }
            }
            return grids;
        }
    }
}
