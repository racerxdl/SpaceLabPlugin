using System.Collections.Generic;
using System.Linq;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Torch;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using VRageMath;

namespace SpaceLab
{
    [Category("globalps")]
    public class GlobalGpsCommand : CommandModule
    {
        public SpaceLab Plugin => (SpaceLab)Context.Plugin;
            
        [Command("save", "Global GPS System")]
        [Permission(MyPromoteLevel.None)]
        public void SavePos(string name)
        {
            IMyPlayer player = Context.Player;
            Vector3D playerPos = player.GetPosition();
            GlobalGps _globalGps = Plugin.GetGlobalGPS();
            Persistent<GlobalGpsConfig> _config = _globalGps.GetPersistence();
            
            // TODO: verify if exists with same name
            GlobalGpsConfig_GPS pos = new GlobalGpsConfig_GPS();
            pos.X = playerPos.X;
            pos.Y = playerPos.Y;
            pos.Z = playerPos.Z;
            pos.CreatedBy = player.DisplayName;
            pos.Name = name;
            
            _config.Data.GlobalSavedPositions.Add(pos);
            _config.Save();
            
            Context.Respond("Successful save Global GPS location ("+name+"): "+ playerPos.X +", "+ playerPos.Y +", "+ playerPos.Z);
        }
        
        [Command("get", "Global GPS System")]
        [Permission(MyPromoteLevel.None)]
        public void GetAllPos()
        {
            IMyPlayer player = Context.Player;
            GlobalGps _globalGps = Plugin.GetGlobalGPS();
            Persistent<GlobalGpsConfig> _config = _globalGps.GetPersistence();
            List<GlobalGpsConfig_GPS> gpsList = _config.Data.GlobalSavedPositions.ToList();
            
            foreach (GlobalGpsConfig_GPS gps in gpsList)
            {
                MyGps instanceGps = gps.ToMyGps();
                MySession.Static.Gpss.AddPlayerGps(player.IdentityId, ref instanceGps);
                
                Context.Respond("Create GPS Location ("+gps.Name+") at:"+ gps.X +", "+ gps.Y +", "+ gps.Z);
            }
        }
    }
}