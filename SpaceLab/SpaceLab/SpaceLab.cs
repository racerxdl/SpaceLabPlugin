﻿using NLog;
using System;
using System.IO;
using System.Security.Policy;
using System.Threading;
using System.Windows.Controls;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Plugins;
using Torch.API.Session;
using Torch.Session;

namespace SpaceLab
{
    public class SpaceLab : TorchPluginBase, IWpfPlugin
    {

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly string CONFIG_FILE_NAME = "SpaceLabConfig.cfg";

        private SpaceLabControl _control;
        public UserControl GetControl() => _control ?? (_control = new SpaceLabControl(this));

        private Persistent<SpaceLabConfig> _config;
        public SpaceLabConfig Config => _config?.Data;

        private GlobalGps _globalGps = new GlobalGps();

        private SpaceLabServer server;

        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
            SetupConfig();
            server = new SpaceLabServer(Config.BaseURL, torch, StoragePath);
            _globalGps.Setup(StoragePath);

            var sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (sessionManager != null)
                sessionManager.SessionStateChanged += SessionChanged;
            else
                Log.Warn("No session manager loaded!");

            Save();
        }

        public void ForceReload()
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                Log.Info("Reloading SpaceLab Plugin...");
                server.ForceReload();
                Log.Info("SpaceLab Plugin Reloaded!");
            });
        }

        private void SessionChanged(ITorchSession session, TorchSessionState state)
        {
            
            switch (state)
            {
                case TorchSessionState.Loaded:
                    Log.Info("Session Loaded!");
                    server.RegisterCallbacks();
                    server.LoadAll();
                    server.StartServer();
                    break;

                case TorchSessionState.Unloading:
                    Log.Info("Session Unloading!");
                    server.StopServer();
                    SpaceLabServer.Store.Save();
                    break;
            }
        }

        private void SetupConfig()
        {

            var configFile = Path.Combine(StoragePath, CONFIG_FILE_NAME);

            try
            {

                _config = Persistent<SpaceLabConfig>.Load(configFile);

            }
            catch (Exception e)
            {
                Log.Warn(e);
            }

            if (_config?.Data == null)
            {

                Log.Info("Create Default Config, because none was found!");

                _config = new Persistent<SpaceLabConfig>(configFile, new SpaceLabConfig());
                _config.Save();
            }
        }

        public void Save()
        {
            try
            {
                _config.Save();
                Log.Info("Configuration Saved.");
            }
            catch (IOException e)
            {
                Log.Warn(e, "Configuration failed to save");
            }
        }
        
        public GlobalGps GetGlobalGPS()
        {
            return _globalGps;
        }
    }
}
