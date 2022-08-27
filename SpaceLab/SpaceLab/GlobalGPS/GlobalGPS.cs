using System;
using System.IO;
using NLog;
using Torch;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using VRage.Input;
using VRageMath;
using System.Collections.ObjectModel;

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
    }
}