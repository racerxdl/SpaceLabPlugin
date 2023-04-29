using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceLabAPI.Models;

namespace SpaceLabAPI.Extensions
{
    public static class ExtendedFactions
    {
        public static List<Faction> GetFactions(this MyFactionCollection collection)
        {
            var factions = collection.GetAllFactions();
            return factions.Select(f => f.Faction()).ToList();
        }

        public static string GetPlayerFactionName(this MyFactionCollection collection, long playerId)
        {
            var faction = collection.TryGetPlayerFaction(playerId);
            return faction?.Name ?? "NONE";
        }

        public static string[] GetMembers(this MyFaction f)
        {
            return f.Members
                .Select((kv) => kv.Value)
                .Select((v) => v.PlayerId)
                .Select((id) => MySession.Static.Players.TryGetIdentity(id))
                .Where((p) => p != null)
                .Select((p) => p.DisplayName)
                .ToArray();
        }

        public static Faction Faction(this MyFaction f)
        {
            return new Faction
            {
                Name = f.Name,
                Tag = f.Tag,
                PublicNote = f.Description,
                FounderName = MySession.Static.Players.TryGetIdentity(f.FounderId)?.DisplayName ?? "Unavailable",
                Members = f.GetMembers()
            };
        }
    }
}
