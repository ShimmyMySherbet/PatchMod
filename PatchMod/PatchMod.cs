using PatchMod.Components;
using PatchMod.Models;
using PatchMod.Modules;
using SDG.Framework.Modules;
using System;
using System.Runtime.CompilerServices;

namespace PatchMod
{
    public static class PatchMod
    {
        public static INIFile Config;
    }

    public class Initializer : IModuleNexus
    {
        public void initialize()
        {
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
            }
            LogClient.LogMessage("PatchMod.SyncStop");
            Console.ReadLine();
        }

        public void Init()
        {
            LogClient.LogMessage("Loading PatchMod");
            PathHelpers.CheckDirectories();
            LogClient.LogMessage("PatchMod Loaded");
            Patcher.Patch();

            if ((bool)PatchMod.Config["SyncEnabled", typeof(bool)])
            {
                LogClient.LogMessage($"Verifying Sync...");
                SyncSource S = SyncProviderManager.GetSource((string)PatchMod.Config["SyncMode"]);
                if (S == null)
                {
                    LogClient.LogMessage($"Invalid Sync Source '{(string)PatchMod.Config["SyncMode"]}'");
                } else
                {
                    S.Source = (string)PatchMod.Config["SyncPath"];
                    S.Init();
                    FileSync.SyncFrom(S);
                    LogClient.LogMessage($"Sync Check Complete.");
                }
            }

            if (Patcher.PatchMessageSent)
            {
                LogClient.LogMessage($"Patches Complete");
            }

        }

        public void shutdown()
        {
            LogClient.LogMessage("PatchMod Shutting down...");
        }
    }
}