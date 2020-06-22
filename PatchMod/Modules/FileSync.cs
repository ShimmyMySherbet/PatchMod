using PatchMod.Models;
using System.IO;

namespace PatchMod.Modules
{
    public static class FileSync
    {
        public static SyncExclusionList LoadExclusions()
        {
            var List = new SyncExclusionList();
            List.LoadFromFile(PathHelpers.PathModSyncExclusions);
            return List;
        }

        public static void SyncFrom(SyncSource Source)
        {
            SyncExclusionList Exclusions = LoadExclusions();
            foreach (SyncFile LFile in Source.GetFiles(""))
            {
                if (!Exclusions.SyncPath(LFile.Path)) continue;

                string RealPath = Path.Combine(PathHelpers.RocketDirectory, LFile.Name);

                if (!File.Exists(RealPath) || !Source.CompareFiles(LFile.Path, RealPath))
                {
                    LogClient.LogMessage($"File requires update: {LFile.Path}");
                    File.Delete(RealPath);
                    using (Stream FStream = LFile.GetStream())
                    using (FileStream WriteStream = new FileStream(RealPath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        FStream.CopyTo(WriteStream);
                    }
                    LogClient.LogMessage($"Updated: {LFile.Path}");
                }
                else
                {
                    LogClient.LogMessage($"File is up to date: {LFile.Path}");
                }
            }

            foreach (SyncDirectory Dir in Source.GetDirectories(""))
            {
                string RealPath = Path.Combine(PathHelpers.RocketDirectory, Dir.Name);
                if (Exclusions.SyncPath(Dir.Path))
                {
                    if (!Directory.Exists(RealPath))
                    {
                        LogClient.LogMessage($"Creating Directory: {Dir.Path}");
                        Directory.CreateDirectory(RealPath);
                    }
                }
            }
        }

        public static void SyncDirectory(SyncSource Source, string Dir, SyncExclusionList Exclusions)
        {
            foreach (SyncFile LFile in Source.GetFiles(Dir))
            {
                if (!Exclusions.SyncPath(LFile.Path)) continue;
                string RealPath = Path.Combine(PathHelpers.RocketDirectory, Dir, LFile.Name);

                if (!File.Exists(RealPath) || !Source.CompareFiles(LFile.Path, RealPath))
                {
                    LogClient.LogMessage($"File requires update: {LFile.Path}");
                    File.Delete(RealPath);
                    LogClient.LogMessage($"Updating: {LFile.Path}");
                    using (Stream FStream = LFile.GetStream())
                    using (FileStream WriteStream = new FileStream(RealPath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        FStream.CopyTo(WriteStream);
                    }
                    LogClient.LogMessage($"Updated: {LFile.Path}");
                }
                else
                {
                    LogClient.LogMessage($"File is up to date: {LFile.Path}");
                }
            }

            foreach (SyncDirectory SDir in Source.GetDirectories(Dir))
            {
                string RealPath = Path.Combine(PathHelpers.RocketDirectory, Dir, SDir.Name);
                if (Exclusions.SyncPath(SDir.Path))
                {
                    if (!Directory.Exists(RealPath))
                    {
                        LogClient.LogMessage($"Creating Directory: {SDir.Path}");
                        Directory.CreateDirectory(RealPath);
                    }
                    SyncDirectory(Source, SDir.Path, Exclusions);
                }
            }
        }
    }
}