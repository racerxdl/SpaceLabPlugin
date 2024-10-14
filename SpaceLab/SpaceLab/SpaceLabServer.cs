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
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using SpaceLab.Models;
using VRage.Game.ModAPI;
using SpaceLab.Extensions;
using Torch.API;
using VRage.GameServices;

namespace SpaceLab
{
    public class SpaceLabServer
    {
        public static SpaceStore Store = new SpaceStore();
        static readonly Logger Log = LogManager.GetCurrentClassLogger();
        static List<SpaceLabMessage> messages;
        static Mutex messageMtx;
        bool running = false;
        private readonly Timer tickTimer;
        private ITorchBase Torch;

        SharpBoss.SharpBoss sharpBoss;

        public SpaceLabServer(string URL, ITorchBase torch, string StoragePath)
        {
            Torch = torch;
            Log.Info($"Creating sharpboss instance with BaseURL: {URL}");
            sharpBoss = new SharpBoss.SharpBoss(URL);
            Log.Info("sharpboss instance created");
            messages = new List<SpaceLabMessage>();
            messageMtx = new Mutex();
            tickTimer = new Timer(Tick, null, 0, 500);
            Store.Setup(StoragePath);
        }

        private void Tick(object state)
        {
            if (running)
            {
                Torch.InvokeBlocking(() => { Store.Tick(); });
            }
        }

        public void RegisterCallbacks()
        {
            // Register callbacks
            Log.Info($"Registering callbacks");
            MyMultiplayer.Static.ChatMessageReceived += Static_ChatMessageReceived;
            MyCubeGridGroups.Static.OnGridGroupCreated += Static_OnGridGroupCreated;
            MyCubeGridGroups.Static.OnGridGroupDestroyed += Static_OnGridGroupDestroyed;
            MySession.Static.Players.PlayerCharacterDied += Players_PlayerCharacterDied;
        }

        private void Players_PlayerCharacterDied(long characterEntityId)
        {
            Store.PlayerDeath(characterEntityId);
        }

        public void LoadAll()
        {
            // Load all grids
            Log.Info($"Loading grids");
            var currentGrids = MyCubeGridGroups.Static.Physical.Groups.ToList();
            foreach (var group in currentGrids)
            {
                Log.Info($"Loading group {group.GroupData} with {group.Nodes.Count} grids");
                Static_OnGridGroupCreated(group.GroupData);
                foreach (var groupNodes in group.Nodes)
                {
                    Log.Info($"Loading grid {groupNodes.NodeData.DisplayName}");
                    Obj_OnGridAdded(group.GroupData, groupNodes.NodeData, null);
                }
            }

            Log.Info($"Loading voxels");
            var voxels = MySession.Static.VoxelMaps.Instances.ToList();
            foreach (var voxel in voxels)
            {
                Log.Info($"Loading voxel {voxel.StorageName} - {voxel.DisplayName} - {voxel.DebugName}");
                Store.AddVoxel(voxel.ToSpaceVoxel());
            }
        }


        public void ForceReload()
        {
            sharpBoss.ForceReload();
        }

        public void StartServer()
        {
            if (!running)
            {
                Log.Info("Starting sharpboss instance");
                running = true;
                sharpBoss.Run();
            }
        }

        private void Static_OnGridGroupDestroyed(IMyGridGroupData gridGroup)
        {
            if (gridGroup is MyGridPhysicalGroupData)
            {
                // Log.Info($"GridGroup removed {gridGroup} ({gridGroup.GetType().FullName})");
                gridGroup.OnGridAdded -= Obj_OnGridAdded;
                gridGroup.OnGridRemoved -= Obj_OnGridRemoved;
                Store.RemoveRelativeGroup(gridGroup);
            }
        }

        private void Static_OnGridGroupCreated(IMyGridGroupData gridGroup)
        {
            if (gridGroup is MyGridPhysicalGroupData)
            {
                // Log.Debug($"GridGroup added {gridGroup}");
                gridGroup.OnGridAdded += Obj_OnGridAdded;
                gridGroup.OnGridRemoved += Obj_OnGridRemoved;
                gridGroup.OnReleased += (group) =>
                {
                    gridGroup.OnGridAdded -= Obj_OnGridAdded;
                    gridGroup.OnGridRemoved -= Obj_OnGridRemoved;
                };
                Store.GetOrAddRelativeGroup(gridGroup);
            }
        }

        private void Obj_OnGridRemoved(IMyGridGroupData thisGrid, IMyCubeGrid removedGrid, IMyGridGroupData previousGroup)
        {
            // Store.RemoveGrid(removedGrid.EntityId.ToString());
        }

        private void Obj_OnGridAdded(IMyGridGroupData thisGrid, IMyCubeGrid addedGrid, IMyGridGroupData previousGroup)
        {
            // Log.Debug($"Grid added {addedGrid.EntityId} ({addedGrid.GetType().FullName}");
            if (!(addedGrid is MyCubeGrid grid))
                return;

            var relGroupId = Store.GetOrAddRelativeGroup(thisGrid);
            Store.AddGrid(grid, relGroupId);
        }

        private void Static_ChatMessageReceived(ulong steamUserId, string messageText, Sandbox.Game.Gui.ChatChannel channel, long _, ChatMessageCustomData? customData)
        {
            MyPlayer player;
            if (!MySession.Static.Players.TryGetPlayerBySteamId(steamUserId, out player))
            {
                player = null;
            }

            Log.Debug($"Chat ({steamUserId}) {customData?.AuthorName}: {messageText}");
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
