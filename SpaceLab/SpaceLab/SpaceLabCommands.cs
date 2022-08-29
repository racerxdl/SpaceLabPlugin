using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using VRageMath;
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

    }
}
