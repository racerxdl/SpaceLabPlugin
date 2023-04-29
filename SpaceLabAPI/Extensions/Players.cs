using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using SpaceLabAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace SpaceLabAPI.Extensions
{
    public static class ExtendedPlayers
    {
        public static bool PlayerIsOnline(this Dictionary<long, MyPlayer> collection, long playerID)
        {
            var valueOrDefault = collection.GetValueOrDefault(playerID);
            return valueOrDefault != null;
        }

        public static Dictionary<long, MyPlayer> GetDictOnlinePlayers(this MyPlayerCollection collection)
        {
            var playerDict = new Dictionary<long, MyPlayer>();

            var onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
            onlinePlayers.ForEach((p) => { playerDict[p.Identity.IdentityId] = p; });

            return playerDict;
        }

        public static List<Player> GetIdentity(this MyPlayerCollection collection)
        {
            var players = collection.GetAllIdentities();
            var onlinePlayers = collection.GetDictOnlinePlayers();

            return players.Where((p) => p != null).Select(p => p.Player(onlinePlayers)).ToList();
        }

        public static Player Player(this MyIdentity p, Dictionary<long, MyPlayer> onlinePlayers)
        {
            return new Player
            {
                Id = p.IdentityId.ToString(),
                SteamId = onlinePlayers.GetValueOrDefault(p.IdentityId)?.Id.SteamId.ToString() ?? "",
                Name = p.DisplayName,
                Faction = MySession.Static.Factions.GetPlayerFactionName(p.IdentityId),
                IsOnline = onlinePlayers.PlayerIsOnline(p.IdentityId),
                Position = onlinePlayers.GetValueOrDefault(p.IdentityId)?.GetPosition() ?? new Vector3D()
            };
        }
    }
}
