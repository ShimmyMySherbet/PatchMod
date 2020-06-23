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
                LogClient.LogMessage($"Failed to sync: {ex.Message}");
            }
            Console.ReadLine();
        }

        public void Init()
        {
            LogClient.LogMessage("Loading PatchMod");
            LibManager.LoadDeps();
            PathHelpers.CheckDirectories();
            LibManager.LoadPlugins();
            LogClient.LogMessage("PatchMod Loaded");
            Patcher.Patch();

            if (Patcher.PatchMessageSent)
            {
                LogClient.LogMessage($"Patches Complete");
            }

            if ((bool)PatchMod.Config["SyncEnabled", typeof(bool)])
            {
                SyncSource S = SyncProviderManager.GetSource((string)PatchMod.Config["SyncMode"]);
                if (S == null)
                {
                    LogClient.LogMessage($"Invalid Sync Source '{(string)PatchMod.Config["SyncMode"]}'");
                } else
                {
                    S.Source = (string)PatchMod.Config["SyncPath"];
                    S.Init();
                    LogClient.LogMessage($"Starting Sync...");
                    FileSync.SyncFrom(S);
                    if (S.FilesChanged)
                    {
                        LogClient.LogMessage($"Files synced from server. Acquired {S.NewFiles} new file/s from server.");
                    } else
                    {
                        LogClient.LogMessage($"Files are up to date.");

                    }
                    S.Shutdown();
                }
            }


        }

        public void shutdown()
        {
            LogClient.LogMessage("PatchMod Shutting down...");
        }
    }
}