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


        [Command("bot", "Chat Bot System: Ask GPT for anything")]
        public void ChatBot(string message)
        {
            Context.Respond(message);
        }
    }
}
