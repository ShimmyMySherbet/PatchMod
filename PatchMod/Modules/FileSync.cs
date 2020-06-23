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
                    bool exists = File.Exists(RealPath);
                    File.Delete(RealPath);
                    using (Stream RemoteStream = LFile.GetStream())
                    using (FileStream LocalStream = new FileStream(RealPath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        RemoteStream.CopyTo(LocalStream);
                        LocalStream.Flush();
                        LocalStream.Close();
                    }
                    Source.FilesChanged = true;
                    Source.NewFiles += 1;
                    if (exists)
                    {
                        LogClient.LogMessage($"[Sync][Updated][File]: {LFile.Path}");
                    } else
                    {
                        LogClient.LogMessage($"[Sync][Created][File]: {LFile.Path}");
                    }
                }
            }

            foreach (SyncDirectory Dir in Source.GetDirectories(""))
            {
                string RealPath = Path.Combine(PathHelpers.RocketDirectory, Dir.Name);
                if (Exclusions.SyncPath(Dir.Path))
                {
                    if (!Directory.Exists(RealPath))
                    {
                        Source.FilesChanged = true;
                        Directory.CreateDirectory(RealPath);
                        LogClient.LogMessage($"[Sync][Created][Dir] :  {Dir.Path}");

                    }
                    SyncDirectory(Source, Dir.Path, Exclusions);
                }

            }
        }

        public static void SyncDirectory(SyncSource Source, string Dir, SyncExclusionList Exclusions)
        {
            foreach (SyncFile LFile in Source.GetFiles(Dir))
            {
                if (!Exclusions.SyncPath(LFile.Path)) continue;
                string RealPath = Path.Combine(PathHelpers.RocketDirectory + Dir.Replace('/', '\\'), LFile.Name.Replace('/', '\\'));
                if (!File.Exists(RealPath) || !Source.CompareFiles(LFile.Path, RealPath))
                {
                    bool exists = File.Exists(RealPath);
                    File.Delete(RealPath);
                    using (Stream RemoteStream = LFile.GetStream())
                    using (FileStream LocalStream = new FileStream(RealPath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        RemoteStream.CopyTo(LocalStream);
                        LocalStream.Flush();
                        LocalStream.Close();
                    }
                    Source.FilesChanged = true;
                    Source.NewFiles += 1;
                    if (exists)
                    {
                        LogClient.LogMessage($"[Sync][Updated][File]:  {LFile.Path}");
                    }
                    else
                    {
                        LogClient.LogMessage($"[Sync][Created][File]:  {LFile.Path}");
                    }
                }
            }

            foreach (SyncDirectory SDir in Source.GetDirectories(Dir))
            {
                string RealPath = Path.Combine(PathHelpers.RocketDirectory + Dir.Replace('/', '\\'), SDir.Name);
                if (Exclusions.SyncPath(SDir.Path))
                {
                    if (!Directory.Exists(RealPath))
                    {
                        Directory.CreateDirectory(RealPath);
                        LogClient.LogMessage($"[Sync][Created][Dir] :  {SDir.Path}");
                        Source.FilesChanged = true;
                    }
                    SyncDirectory(Source, SDir.Path, Exclusions);
                }
            }
        }
    }
}