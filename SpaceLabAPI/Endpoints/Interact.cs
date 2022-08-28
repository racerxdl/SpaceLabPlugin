using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using SharpBoss.Attributes;
using SharpBoss.Attributes.Methods;
using SharpBoss.Logging;
using SpaceLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceLabAPI.Endpoints
{
    [REST("/interact")]
    public class Interact
    {

        [POST("/sendmessage")]
        public bool SendMessage(SpaceLabMessage message)
        {
            MyMultiplayer.Static.SendChatMessage(message.Message, Sandbox.Game.Gui.ChatChannel.Global, 0, message.From);
            Logger.Info($"Sending message [{message.From}] - {message.Message}");
            return true;
        }

        [GET("/chat")]
        public List<SpaceLabMessage> GetMessages()
        {
            return SpaceLabServer.GetMessages();
        }
    }
}
