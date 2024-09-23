using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using NLog;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using SpaceLab.Models;
using Torch;
using VRage.Game.ModAPI;

namespace SpaceLab
{
    public class SpaceStore
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private Persistent<StorePersist> _storedData;

        public ConcurrentDictionary<string, Grid> Grids { get; } = new ConcurrentDictionary<string, Grid>();
        public ConcurrentDictionary<IMyGridGroupData, int> RelativeGroups { get; } = new ConcurrentDictionary<IMyGridGroupData, int>();
        public ConcurrentDictionary<string, Player> Players { get; } = new ConcurrentDictionary<string, Player>();
        public List<Voxel> Voxels = new List<Voxel>();

        private int _lastId;
        private DateTime _lastGridUpdate;
        private DateTime _lastSave;

        public event OnGridAddedHandler OnGridAdded;
        public event OnGridRemovedHandler OnGridRemoved;

        public void Setup(string storagePath)
        {
            var configFile = Path.Combine(storagePath, "SpaceStore");

            try
            {
                _storedData = Persistent<StorePersist>.Load(configFile);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                _storedData = new Persistent<StorePersist>(configFile, new StorePersist());
                _storedData.Save();
            }

            foreach (var player in _storedData.Data.Players)
                Players[player.Id] = player;
        }


        public void AddGrid(MyCubeGrid grid, int relGroupId)
        {
            Log.Info($"New grid added: {grid.Name} ({grid.EntityId})");
            var ngrid = new Grid(grid, relGroupId);
            grid.OnClose += (entity) => RemoveGrid(entity.EntityId.ToString());
            Grids[ngrid.Id] = ngrid;
            OnGridAdded?.Invoke(this, ngrid);
        }

        public void RemoveGrid(string id)
        {
            if (!Grids.ContainsKey(id))
                return;

            Log.Info($"Grid removed: {Grids[id].Name} ({id})");
            var grid = Grids[id];
            Grids.Remove(id);
            OnGridRemoved?.Invoke(this, grid);
            grid.Dispose();

        }

        public int GetOrAddRelativeGroup(IMyGridGroupData group)
        {
            if (RelativeGroups.ContainsKey(group))
                return RelativeGroups[group];
            _lastId++;
            RelativeGroups[group] = _lastId;
            return _lastId;
        }
        
        public void RemoveRelativeGroup(IMyGridGroupData group)
        {
            if (!RelativeGroups.ContainsKey(group))
                return;
            RelativeGroups.Remove(group);
        }

        public void AddVoxel(Voxel voxel)
        {
            if (voxel == null)
                return;

            Voxels.Add(voxel);
        }

        public void PlayerDeath(long lid)
        {
            var id = lid.ToString();
            if (!Players.ContainsKey(id))
                return;

            Players[id].Deaths++;
            Log.Info($"Player {Players[id].Name} died {Players[id].Deaths} times...");
            _storedData.Data.UpdatePlayer(Players[id]);
        }

        public void Tick()
        {
            if (DateTime.Now - _lastGridUpdate < TimeSpan.FromSeconds(1))
                return;

            try
            {
                foreach (var item in Grids)
                    item.Value.Update();
            } catch (Exception e)
            {
                Log.Error(e);
            }


            // Update Players
            var players = MySession.Static.Players.GetAllIdentities();
            var onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
            var mapOnline = new Dictionary<long, MyPlayer>();

            foreach (var player in onlinePlayers)
            {
                if (player == null)
                    continue;
                mapOnline[player.Identity.IdentityId] = player;
            }

            foreach (var player in players)
            {
                if (player == null)
                    continue;

                var id = player.IdentityId.ToString();
                var pdata = mapOnline.TryGetValue(player.IdentityId, out var p) ? p : null;
                var faction = MySession.Static.Factions.TryGetPlayerFaction(player.IdentityId);

                if (!Players.ContainsKey(id))
                {
                    if (pdata == null)
                        continue;

                    var sp = new Player
                    {
                        Id = id,
                        Name = pdata.DisplayName,
                        Position= pdata?.GetPosition() ?? new VRageMath.Vector3D(),
                        IsOnline = pdata != null,
                        SteamId = pdata?.Id.SteamId.ToString() ?? "",
                        Faction = faction?.Name ?? "NONE",
                    };
                    Players[id] = sp;
                    _storedData.Data.AddPlayer(sp);
                } 
                else
                {
                    Players[id].Position = pdata?.GetPosition() ?? new VRageMath.Vector3D();
                    Players[id].IsOnline = pdata != null;
                    Players[id].Faction = faction?.Name ?? "NONE";
                    _storedData.Data.UpdatePlayer(Players[id]);
                }
            }

            if (DateTime.Now - _lastSave > TimeSpan.FromMinutes(1))
            {
                Save();
                _lastSave = DateTime.Now;
            }

            _lastGridUpdate = DateTime.Now;
        }

        public void Save()
        {
            try
            {
                // _storedData.Save();
                // Disabled for now
            } catch (IOException e)
            {
                Log.Error(e);
            }
        }
    }

    public delegate void OnGridRemovedHandler(object sender, Grid grid);

    public delegate void OnGridAddedHandler(object sender, Grid grid);
}