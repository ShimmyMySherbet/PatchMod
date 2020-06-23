using System;
using System.IO;

namespace PatchMod.Modules
{
    public static class PathHelpers
    {
        public static readonly string UnturnedDirectory = Environment.CurrentDirectory;

        public static string ServersDirectory
        {
            get
            {
                return Path.Combine(UnturnedDirectory, "Servers");
            }
        }

        public static string ModulesDirectory
        {
            get
            {
                return Path.Combine(UnturnedDirectory, "Modules");
            }
        }

        public static string PatchModModuleDirectory
        {
            get
            {
                return Path.Combine(ModulesDirectory, "PatchMod");
            }
        }



        public static string PatchModDepDirectory
        {
            get
            {
                return Path.Combine(PatchModModuleDirectory, "Lib");
            }
        }


        public static string PatchModPluginsDirectory
        {
            get
            {
                return Path.Combine(PatchModModuleDirectory, "SyncProviders");
            }
        }


        public static string ServerDirectory
        {
            get
            {
                return Path.Combine(ServersDirectory, SDG.Unturned.Provider.serverID);
            }
        }

        public static string PatchModDirectory
        {
            get
            {
                return Path.Combine(ServerDirectory, "PatchMod");
            }
        }

        public static string PatchModPatchDirectory
        {
            get
            {
                return Path.Combine(ServerDirectory, "PatchMod", "Patch");
            }
        }

        public static string RocketDirectory
        {
            get
            {
                return Path.Combine(ServerDirectory, "Rocket");
            }
        }

        public static string PatchModLibrariesDirectory
        {
            get
            {
                return Path.Combine(PatchModPatchDirectory, "Libraries");
            }
        }

        public static string PatchModPatchPluginsDirectory
        {
            get
            {
                return Path.Combine(PatchModPatchDirectory, "Plugins");
            }
        }

        public static string PatchModLogs
        {
            get
            {
                return Path.Combine(PatchModDirectory, "Patchmod.log");
            }
        }

        public static string PatchModConfig
        {
            get
            {
                return Path.Combine(PatchModDirectory, "Config.ini");
            }
        }

        public static string PathModSyncExclusions
        {
            get
            {
                return Path.Combine(PatchModDirectory, ".SyncExclude");
            }
        }

        public static void CheckDirectories()
        {
            if (!Directory.Exists(PatchModPluginsDirectory))
                Directory.CreateDirectory(PatchModPluginsDirectory);

            if (!Directory.Exists(PatchModDirectory))
                Directory.CreateDirectory(PatchModDirectory);
            if (!Directory.Exists(PatchModPatchDirectory))
                Directory.CreateDirectory(PatchModPatchDirectory);
            if (!Directory.Exists(PatchModLibrariesDirectory))
                Directory.CreateDirectory(PatchModLibrariesDirectory);
            if (!Directory.Exists(PatchModPatchPluginsDirectory))
                Directory.CreateDirectory(PatchModPatchPluginsDirectory);
            if (!File.Exists(Path.Combine(PatchModPatchDirectory, "patchmod.remlist.txt")))
                File.WriteAllText(Path.Combine(PatchModPatchDirectory, "patchmod.remlist.txt"), "#Put paths to file/folders to delete in here.\n");
            if (!File.Exists(PathModSyncExclusions))
                File.WriteAllText(PathModSyncExclusions, "#Put paths to files/folders in here to exclude from syncing. Prefix paths with '!' to sync them.\n");
            if (!File.Exists(PatchModConfig))
            {
                PatchMod.Config = ConfigBuilder.CreateDefaultConfig();
                PatchMod.Config.Save(PatchModConfig);
            }
            else
            {
                PatchMod.Config = new Components.INIFile(PatchModConfig);
                ConfigBuilder.CheckConfig(PatchMod.Config);
                if (PatchMod.Config.HasUnsavedChanges) PatchMod.Config.Save();
            }
        }

        public static string GetRelativePath(string BaseDirectory, string InnerScope)
        {
            try
            {
                DirectoryInfo BaseSpec = new DirectoryInfo(BaseDirectory);
                DirectoryInfo ScopeSpec = new DirectoryInfo(InnerScope);
                return ScopeSpec.FullName.Remove(0, BaseSpec.FullName.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return "";
            }
        }
    }
}