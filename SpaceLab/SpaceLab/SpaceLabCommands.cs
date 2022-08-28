using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using Sandbox.Game.World;
using Sandbox.Game.Entities;
using System.Linq;
using VRage.Groups;
using Sandbox.Game.Screens.Helpers;

namespace SpaceLab
{
    [Category("SpaceLab")]
    public class SpaceLabCommands : CommandModule
    {

        public SpaceLab Plugin => (SpaceLab)Context.Plugin;

        [Command("test", "This is a Test Command.")]
        [Permission(MyPromoteLevel.Moderator)]
        public void Test()
        {
            Context.Respond("This is a Test from " + Context.Player);
        }

        [Command("testWithCommands", "This is a Test Command.")]
        [Permission(MyPromoteLevel.None)]
        public void TestWithArgs(string foo = null, string bar = null)
        {
            Context.Respond("This is a Test " + foo + ", " + bar);
            foreach (var group in MyCubeGridGroups.Static.Physical.Groups.ToList())
            {
                foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node groupNodes in group.Nodes)
                {
                    MyCubeGrid grid = groupNodes.NodeData;
                    var ownerNames = grid.BigOwners.Select((id) => MySession.Static.Players.TryGetIdentity(id).DisplayName).ToList();
                    var ownerName = ownerNames.Count > 0 ? ownerNames[0] : "NONE";
                    Context.Respond($"Grid: {grid.DisplayName} - {grid.BlocksCount} - {ownerName} - {grid.WorldMatrix.Translation}");
                }
            }
            foreach (var voxmap in MySession.Static.VoxelMaps.Instances.ToList()) {
                Context.Respond($"Voxel Map: {voxmap.EntityId} - {voxmap.Name} - {voxmap.WorldMatrix.Translation}");
            }
            
            foreach (var player in MySession.Static.Players.GetOnlinePlayers())
            {
                Context.Respond($"Player: {player.DisplayName} - {player.GetPosition()}");
            }
            

        }
    }
}
