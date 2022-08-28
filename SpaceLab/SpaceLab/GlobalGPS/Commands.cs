using System.Collections.Generic;
using System.Linq;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.ModAPI;
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
        private SpaceLab Plugin => (SpaceLab)Context.Plugin;
            
        [Command("save", "Global GPS System")]
        [Permission(MyPromoteLevel.None)]
        public void SavePos(string name)
        {
            var player = Context.Player;
            var playerPos = player.GetPosition();
            var config = Plugin.GetGlobalGPS().GetPersistence();
            
            if (config.Data.GlobalSavedPositions.ToList().Any(gpsMark => gpsMark.Name == name))
            {
                Context.Respond("You cant overwrite a existent gps mark");
                return;
            }

            var pos = new GlobalGpsConfig_GPS(playerPos, name, player.DisplayName);
            GlobalGps.MarkGpsToPlayer(player.IdentityId, pos.ToMyGps());
            
            config.Data.GlobalSavedPositions.Add(pos);
            config.Save();
            
            Context.Respond("Successful save Global GPS location ("+name+"): "+ playerPos.X +", "+ playerPos.Y +", "+ playerPos.Z);
        }
        
        [Command("get", "Global GPS System")]
        [Permission(MyPromoteLevel.None)]
        public void GetAllPos()
        {
            var player = Context.Player;
            var globalGps = Plugin.GetGlobalGPS();
            var config = globalGps.GetPersistence();
            
            config.Data.GlobalSavedPositions.ForEach(gps => GlobalGps.MarkGpsToPlayer(player.IdentityId, gps.ToMyGps()));
            Context.Respond("Successful get all global gps location");
        }
    }
}