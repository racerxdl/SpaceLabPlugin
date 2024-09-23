using NLog;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using VRageMath;

namespace SpaceLab.Models
{
    public class Grid
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        private readonly MyCubeGrid _refGrid;
        private readonly Guid guid = Guid.NewGuid();

        public string Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Faction { get; set; }
        public string FactionTag { get; set; }
        public int Blocks { get; set; }
        public bool IsPowered { get; set; }
        public bool AntenaIsWorking { get; set; }
        public bool HasAntena { get; set; }
        public double GridSize { get; set; }
        public bool IsStatic { get; set; }
        public bool IsParked { get; set; }
        public string ParentId { get; set; }
        public int RelGroupId { get; set; }

        /// <summary>
        /// <deprecated>Always 0</deprecated>
        /// </summary>
        public int RelGroupCount { get; } = 0;

        public int PCU { get; set; }
        public Vector3D Position { get; set; }
        public Quaternion Rotation { get; set; }

        public double X
        {
            get { return Position.X; }
        }

        public double Y
        {
            get { return Position.Y; }
        }

        public double Z
        {
            get { return Position.Z; }
        }

        public Grid() { }

        public void Dispose()
        {
            if (_refGrid != null)
            {
                Log.Debug($"Disposing grid {Name} ({Id}) callbacks");
                _refGrid.OnGridChanged -= OnGridChanged;
                _refGrid.OnBlockOwnershipChanged -= OnBlockOwnershipChanged;
                _refGrid.OnFatBlockAdded -= OnFatBlockAdded;
                _refGrid.OnFatBlockRemoved -= OnFatBlockRemoved;
            }
        }

        internal Grid(MyCubeGrid grid, int relGroupId)
        {
            _refGrid = grid;
            RelGroupId = relGroupId;

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

            Id = grid.EntityId.ToString();
            Owner = ownerName;
            Faction = factionName;
            FactionTag = factionTag;
            GridSize = grid.GridSize;

            OnGridChanged(grid);

            // Grab antenna
            foreach (var block in grid.GetFatBlocks())
            {
                if (block is IMyRadioAntenna antenna)
                {
                    AntenaIsWorking = antenna.IsWorking;
                    HasAntena = true;
                    antenna.PropertiesChanged -= OnAntennaPropertyChanged; // Avoid double registration
                    antenna.PropertiesChanged += OnAntennaPropertyChanged;
                    break;
                }
            }

            Log.Debug($"Registering grid {Name} ({Id}) callbacks");
            _refGrid.OnGridChanged -= OnGridChanged;
            _refGrid.OnBlockOwnershipChanged -= OnBlockOwnershipChanged;
            _refGrid.OnFatBlockAdded -= OnFatBlockAdded;
            _refGrid.OnFatBlockRemoved -= OnFatBlockRemoved;

            _refGrid.OnGridChanged += OnGridChanged;
            _refGrid.OnBlockOwnershipChanged += OnBlockOwnershipChanged;
            _refGrid.OnFatBlockAdded += OnFatBlockAdded;
            _refGrid.OnFatBlockRemoved += OnFatBlockRemoved;
        }

        private void OnFatBlockRemoved(MyCubeBlock block)
        {
            Log.Debug($"({guid}) Block removed {block}");
            if (block is IMyRadioAntenna antenna)
            {
                antenna.PropertiesChanged -= OnAntennaPropertyChanged;
                HasAntena = false;
            }
        }

        private void OnFatBlockAdded(MyCubeBlock block)
        {
            Log.Debug($"({guid}) Block added {block}");
            if (block is IMyRadioAntenna antenna)
            {
                antenna.PropertiesChanged -= OnAntennaPropertyChanged;
                antenna.PropertiesChanged += OnAntennaPropertyChanged;
                HasAntena = true;
            }
        }

        private void OnAntennaPropertyChanged(IMyTerminalBlock entity)
        {
            if (entity is IMyRadioAntenna antenna && AntenaIsWorking != antenna.IsWorking)
            {
                Log.Debug($"({guid}) Antenna property changed {antenna.IsWorking}");
                AntenaIsWorking = antenna.IsWorking;
            }
        }

        private void OnBlockOwnershipChanged(MyCubeGrid grid)
        {
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

            Owner = ownerName;
            Faction = factionName;
            FactionTag = factionTag;
        }

        private void OnGridChanged(MyCubeGrid grid)
        {
            Name = grid.DisplayName;
            Position = grid.WorldMatrix.Translation;
            Rotation = Quaternion.CreateFromRotationMatrix(grid.WorldMatrix.GetOrientation());
            Blocks = grid.BlocksCount;
            IsPowered = grid.IsPowered;
            IsStatic = grid.IsStatic;
            IsParked = grid.IsParked;
            PCU = grid.BlocksPCU;
            ParentId = grid.Parent?.EntityId.ToString();
        }

        public void Update()
        {
            OnGridChanged(_refGrid);
        }
    }
}