using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpBoss;

namespace SpaceLab
{
    class SpaceLabServer
    {
        static readonly Logger Log = LogManager.GetCurrentClassLogger();

        bool running = false;

        SharpBoss.SharpBoss sharpBoss;

        public SpaceLabServer()
        {
            Log.Info("Creating sharpboss instance");
            sharpBoss = new SharpBoss.SharpBoss("http://*:20000/");
            Log.Info("sharpboss instance created");
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
