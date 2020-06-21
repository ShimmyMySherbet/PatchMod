using System;
using System.IO;

namespace PatchMod.Modules
{
    public static class LogClient
    {
        public static void LogMessage(string Message)
        {
            string msg = $"[{DateTime.Now.ToShortDateString()}][{DateTime.Now.ToShortTimeString()}][Message] {Message}";
            Console.WriteLine($"[PatchMod] {Message}");
            bool LogToR = false;
            bool LogToP = false;
            if (PatchMod.Config != null)
            {
                LogToR = Convert.ToBoolean(PatchMod.Config.ReadValue("LogOutputToRocketModLog"));
                LogToP = Convert.ToBoolean(PatchMod.Config.ReadValue("LogOutputToPatchModLog"));
                if (!Convert.ToBoolean(PatchMod.Config.ReadValue("LogOutput")))
                {
                    LogToP = false;
                    LogToR = false;
                }
            }
            if (LogToP) File.AppendAllLines(PathHelpers.PatchModLogs, new string[] { msg });
            if (LogToR) Rocket.Core.Logging.Logger.Log(msg);
        }
    }
}