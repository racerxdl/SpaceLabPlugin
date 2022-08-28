using System;
using System.IO;
using NLog;
using Torch;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Torch.API.Managers;
using VRageMath;

namespace SpaceLab
{
    public class GlobalGps
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly string CONFIG_FILE_NAME = "SpaceLab_GPS.cfg";
        private Persistent<GlobalGpsConfig> _config;
        
        public void Setup(string storagePath)
        {
            var configFile = Path.Combine(storagePath, CONFIG_FILE_NAME);

            try
            {
                _config = Persistent<GlobalGpsConfig>.Load(configFile);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }

            if (_config?.Data == null)
            {
                Log.Info("Create Default empty GlobalPS file");
                
                _config = new Persistent<GlobalGpsConfig>(configFile, new GlobalGpsConfig());
                _config.Save();
            }
        }

        public Persistent<GlobalGpsConfig> GetPersistence()
        {
            return _config;
        }
        
        public static void MarkGpsToPlayer(long identityId, MyGps gps)
        {
            var gpsCollection = (MyGpsCollection) MyAPIGateway.Session?.GPS;
            gpsCollection?.SendAddGpsRequest(identityId, ref gps, 0L, true);
        }

        public static void MarkGpsToAllPlayers(SpaceLab plugin, MyGps gps)
        {
            var players = MySession.Static.Players.GetAllIdentities();
            foreach (var player in players)
            {
                MarkGpsToPlayer(player.IdentityId, gps);
            }
            
            plugin.Torch.CurrentSession?.Managers?.GetManager<IChatManagerServer>()?.SendMessageAsOther("GlobalPS", "A new gps mark as added to you", Color.Gold);
        }
    }
}