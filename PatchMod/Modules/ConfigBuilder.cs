using PatchMod.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchMod.Modules
{
    public static class ConfigBuilder
    {
        
        public static void CheckConfig(INIFile Config)
        {
            Config.PatchKey("LogOutput", true);
            Config.PatchKey("LogOutputToPatchModLog", true);
            Config.PatchKey("LogOutputToRocketModLog", true);
            Config.PatchKey("SyncEnabled", false);
            Config.PatchKey("SyncPath", "PathToSyncFolder");
            Config.PatchKey("FTPHost", "localhost");
            Config.PatchKey("FTPPort", 21);
            Config.PatchKey("FTPUsername", "Username");
            Config.PatchKey("FTPPassword", "Password");
        }

        public static INIFile CreateDefaultConfig()
        {
            INIFile Config = new INIFile();
            Config.WriteComment("PatchMod Config File.");
            Config.WriteLine();
            Config.WriteComment("Log Settings");
            Config["LogOutput"] = true;
            Config["LogOutputToPatchModLog"] = true;
            Config["LogOutputToRocketModLog"] = true;
            Config.WriteLine();
            Config.WriteComment("Sync Settings; SyncMode: Local or FTP.");
            Config["SyncEnabled"] = false;
            Config["SyncMode"] = "Local";
            Config["SyncPath"] = "Path";
            Config.WriteLine();
            Config.WriteComment("FTP Settings");
            Config["FTPHost"] = "localhost";
            Config["FTPPort"] = 21;
            Config["FTPUsername"] = "Username";
            Config["FTPPassword"] = "Password";
            return Config;
        }


    }
}
