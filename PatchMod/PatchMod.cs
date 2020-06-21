using PatchMod.Components;
using PatchMod.Modules;
using SDG.Framework.Modules;

namespace PatchMod
{
    public static class PatchMod
    {
        public static INIReader Config;
    }

    public class Initializer : IModuleNexus
    {
        public void initialize()
        {
            LogClient.LogMessage("Loading PatchMod");
            PathHelpers.CheckDirectories();
            LogClient.LogMessage("PatchMod Loaded");
            Patcher.Patch();
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