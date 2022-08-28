using System.Linq;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace SpaceLab
{
    [Category("globalps")]
    public class GlobalGpsCommand : CommandModule
    {
        private SpaceLab Plugin => (SpaceLab)Context.Plugin;
            
        [Command("save", "Global GPS System: Save your location")]
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
            GlobalGps.MarkGpsToAllPlayers(Plugin, pos.ToMyGps());
            
            config.Data.GlobalSavedPositions.Add(pos);
            config.Save();
            
            Context.Respond("Successful save Global GPS location ("+name+"): "+ playerPos.X +", "+ playerPos.Y +", "+ playerPos.Z);
        }
        
        [Command("get", "Global GPS System: Get all positions")]
        [Permission(MyPromoteLevel.None)]
        public void GetAllPos()
        {
            var player = Context.Player;
            var globalGps = Plugin.GetGlobalGPS();
            var config = globalGps.GetPersistence();
            
            config.Data.GlobalSavedPositions.ForEach(gps => GlobalGps.MarkGpsToPlayer(player.IdentityId, gps.ToMyGps()));
            Context.Respond("Successful get all global gps location");
        }
        
        // Admin Commands
        [Command("delete", "Global GPS System: Delete a position")]
        [Permission(MyPromoteLevel.Admin)]
        public void DeletePos(string name)
        {
            var globalGps = Plugin.GetGlobalGPS();
            var config = globalGps.GetPersistence();

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