using Sandbox.Game.Entities;
using Sandbox.Game.World;

namespace SpaceLabAPI.Extensions
{
    public static class ExtendedGrid
    {
        public static string GetOwner(this MyCubeGrid grid)
        {
            var ownerId = grid.BigOwners.Count > 0 ? grid.BigOwners[0] : -1;
            if (ownerId == -1) return "NONE";

            return MySession.Static.Players.TryGetIdentity(ownerId)?.DisplayName ?? "NONE";
        }

        public static string GetFactionName(this MyCubeGrid grid)
        {
            var ownerId = grid.BigOwners.Count > 0 ? grid.BigOwners[0] : -1;
            if (ownerId == -1) return "NONE";

            return MySession.Static.Factions.TryGetPlayerFaction(ownerId)?.Name ?? "NONE";
        }

        public static string GetFactionTag(this MyCubeGrid grid)
        {
            var ownerId = grid.BigOwners.Count > 0 ? grid.BigOwners[0] : -1;
            if (ownerId == -1) return "NONE";
            return MySession.Static.Factions.TryGetPlayerFaction(ownerId)?.Tag ?? "NONE";
        }

        public static bool HasAntena(this MyCubeGrid grid, bool isActive)
        {
            foreach (var block in grid.GetFatBlocks())
            {
                if (block is Sandbox.ModAPI.IMyRadioAntenna)
                {
                    var antenna = block as Sandbox.ModAPI.IMyRadioAntenna;

                    if (antenna.IsWorking && isActive)
                        return true;
                    else if (!isActive)
                        return true;
                }
            }

            return false;
        }
    }
}
