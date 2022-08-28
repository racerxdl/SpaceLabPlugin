using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpBoss;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.World;

namespace SpaceLab
{
    public class SpaceLabServer
    {
        static readonly Logger Log = LogManager.GetCurrentClassLogger();
        static List<SpaceLabMessage> messages;
        static Mutex messageMtx;
        bool running = false;

        SharpBoss.SharpBoss sharpBoss;

        public SpaceLabServer()
        {
            Log.Info("Creating sharpboss instance");
            sharpBoss = new SharpBoss.SharpBoss("http://*:20000/");
            Log.Info("sharpboss instance created");
            messages = new List<SpaceLabMessage>();
            messageMtx = new Mutex();
        }

        public void StartServer()
        {
            if (!running)
            {
                Log.Info("Starting sharpboss instance");
                running = true;
                sharpBoss.Run();
                MyMultiplayer.Static.ChatMessageReceived += Static_ChatMessageReceived;
            }
        }

        private void Static_ChatMessageReceived(ulong steamUserId, string messageText, Sandbox.Game.Gui.ChatChannel channel, long targetId, string customAuthorName)
        {
            MyPlayer player;
            if (!MySession.Static.Players.TryGetPlayerBySteamId(steamUserId, out player))
            {
                player = null;
            }

            Log.Debug($"Chat ({steamUserId}) {customAuthorName}: {messageText}");
            if (channel == Sandbox.Game.Gui.ChatChannel.Global && player != null)
            {
                messageMtx.WaitOne();
                messages.Add(new SpaceLabMessage
                {
                    From = player.DisplayName,
                    Message = messageText,
                });
                if (messages.Count > 50)
                {
                    messages.RemoveAt(0);
                }
                messageMtx.ReleaseMutex();
            }
        }

        public static List<SpaceLabMessage> GetMessages()
        {
            var messageList = new List<SpaceLabMessage>();
            if (messages == null)
            {
                return messageList; // To early
            }

            messageMtx.WaitOne();
            messageList.AddList(messages);
            messages.Clear();
            messageMtx.ReleaseMutex();
            return messageList;
        }

        public void StopServer()
        {
            if (running)
            {
                Log.Info("Stopping sharpboss instance");
                running = false;
                sharpBoss.Stop();
            }
        }
    }
}
