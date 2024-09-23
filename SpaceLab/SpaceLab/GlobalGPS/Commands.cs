using System.Linq;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using VRage.Plugins;

namespace SpaceLab
{
    [Category("globalps")]
    public class GlobalGpsCommand : CommandModule
    {
        private SpaceLab Plugin => (SpaceLab)Context.Plugin;

        [Command("get", "Global GPS System: Get all positions")]
        [Permission(MyPromoteLevel.None)]
        public void GetAllPos()
        {
            var plugin = Plugin.GetGlobalGPS();
            var config = plugin.GetPersistence();

            /*if (plugin.IsAllowedToCommand(Context, config.Data.UsagePromoteLevel))
            {
                return;
            }*/

            config.Data.GlobalSavedPositions.ForEach(gps => GlobalGps.MarkGpsToPlayer(Context.Player.IdentityId, gps.ToMyGps()));
            Context.Respond("Successful get all global gps location");
        }

        [Command("save", "Global GPS System: Save your location")]
        [Permission(MyPromoteLevel.None)]
        public void SavePos(string name)
        {
            var plugin = Plugin.GetGlobalGPS();
            var config = plugin.GetPersistence();

            /*if (plugin.IsAllowedToCommand(Context, config.Data.ManagePromoteLevel))
            {
                return;
            }*/

            var player = Context.Player;
            var playerPos = player.GetPosition();
            
            if (config.Data.GlobalSavedPositions.ToList().Any(gpsMark => gpsMark.Name == name))
            {
                Context.Respond("You cant overwrite a existent gps mark");
                return;
            }

            var pos = new GlobalGpsConfig_GPS(playerPos, name, player.DisplayName);
            GlobalGps.MarkGpsToAllPlayers(Plugin, pos.ToMyGps());
            
            config.Data.GlobalSavedPositions.Add(pos);
            config.Save();
            
            Context.Respond("Successful save Global GPS location ("+name+"): "+ playerPos.X +", "+ playerPos.Y +", "+ playerPos.Z);
        }
        
        [Command("delete", "Global GPS System: Delete a position")]
        [Permission(MyPromoteLevel.None)]
        public void DeletePos(string name)
        {
            var plugin = Plugin.GetGlobalGPS();
            var config = plugin.GetPersistence();

            /*if (plugin.IsAllowedToCommand(Context, config.Data.ManagePromoteLevel))
            {
                return;
            }*/

            foreach(var gps in config.Data.GlobalSavedPositions.ToList().Where(gps => gps.Name == name))
            {
                if (config.Data.GlobalSavedPositions.Remove(gps))
                {
                    Context.Respond("Successful delete gps location with name: "+gps.Name);
                }
            }
        }
    }
}