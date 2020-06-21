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

        public static string PatchModPluginsDirectory
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

        public static void CheckDirectories()
        {
            if (!Directory.Exists(PatchModDirectory))
                Directory.CreateDirectory(PatchModDirectory);
            if (!Directory.Exists(PatchModPatchDirectory))
                Directory.CreateDirectory(PatchModPatchDirectory);
            if (!Directory.Exists(PatchModLibrariesDirectory))
                Directory.CreateDirectory(PatchModLibrariesDirectory);
            if (!Directory.Exists(PatchModPluginsDirectory))
                Directory.CreateDirectory(PatchModPluginsDirectory);
            if (!File.Exists(Path.Combine(PatchModPatchDirectory, "patchmod.remlist.txt")))
                File.WriteAllText(Path.Combine(PatchModPatchDirectory, "patchmod.remlist.txt"), "#Put paths to file/folders to delete in here.\n");
            if (!File.Exists(PatchModConfig))
            {
                PatchMod.Config = new Components.INIReader();
                PatchMod.Config.WriteComment("PatchMod Configuration File");
                PatchMod.Config.SetKey("LogOutput", true);
                PatchMod.Config.SetKey("LogOutputToPatchModLog", true);
                PatchMod.Config.SetKey("LogOutputToRocketModLog", true);
                PatchMod.Config.SaveFile(PatchModConfig);
            }
            else
            {
                PatchMod.Config = new Components.INIReader();
                PatchMod.Config.LoadFile(PatchModConfig);
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