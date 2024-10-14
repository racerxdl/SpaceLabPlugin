using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using SharpBoss.Attributes;
using SharpBoss.Attributes.Methods;
using SharpBoss.Logging;
using SpaceLab;
using SpaceLabAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceLab.Models;
using VRage.Groups;

namespace SpaceLabAPI.Endpoints
{
    [REST("/interact")]
    public class Interact
    {
        [POST("/deleteEntity")]
        public string DeleteEntity(List<Grid> grids)
        {
            var result = "Removed: ";
            var gridIds = grids.Select((g) => long.Parse(g.Id)).ToList();
            var gridsToDelete = new List<MyCubeGrid>();
            foreach (var group in MyCubeGridGroups.Static.Physical.Groups.ToList())
            {
                foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node groupNodes in group.Nodes)
                {
                    MyCubeGrid grid = groupNodes.NodeData;
                    if (gridIds.Contains(grid.EntityId))
                    {
                        MyMultiplayer.Static.SendChatMessage($"Erasing entity {grid.DisplayName}-{grid.EntityId}", Sandbox.Game.Gui.ChatChannel.Global, 0, new VRage.GameServices.ChatMessageCustomData
                        {
                            AuthorName = "SERVER",
                            TextColor = VRageMath.Color.Yellow,
                        });
                        Logger.Info($"Erasing entity {grid.DisplayName}-{grid.EntityId}");
                        result += $"{grid.DisplayName}, ";
                        gridsToDelete.Add(grid);
                    }
                }
            }

            gridsToDelete.ForEach((grid) => grid.Close());

            return result;
        }
        [POST("/sendmessage")]
        public bool SendMessage(SpaceLabMessage message)
        {
            MyMultiplayer.Static.SendChatMessage(message.Message, Sandbox.Game.Gui.ChatChannel.Global, 0, new VRage.GameServices.ChatMessageCustomData
            {
                AuthorName = message.From,
            });
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
