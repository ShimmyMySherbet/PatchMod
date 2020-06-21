using System.IO;

namespace PatchMod.Modules
{
    public static class Patcher
    {
        public static bool PatchMessageSent = false;
        public static void CheckSendPatchMessage()
        {
            if (!PatchMessageSent)
            {
                PatchMessageSent = true;
                LogClient.LogMessage($"Patching...");
            }
        }

        public static void Patch()
        {
            string DelFilePath = Path.Combine(PathHelpers.PatchModPatchDirectory, "patchmod.remlist.txt");
           

            foreach (string F in Directory.GetFiles(PathHelpers.PatchModPatchDirectory))
            {
                FileInfo FInfo = new FileInfo(F);
                if (!(FInfo.Name.ToLower() == "patchmod.remlist.txt"))
                {
                    string RealPath = Path.Combine(PathHelpers.RocketDirectory, FInfo.Name);
                    LogClient.LogMessage($"Patching File {FInfo.Name}...");
                    if (File.Exists(RealPath))
                    {
                        CheckSendPatchMessage();
                        File.Delete(RealPath);
                    }

                    File.Move(F, RealPath);
                }
            }

            foreach (string D in Directory.GetDirectories(PathHelpers.PatchModPatchDirectory))
            {
                DirectoryInfo DInfo = new DirectoryInfo(D);
                string RealPath = Path.Combine(PathHelpers.RocketDirectory, DInfo.Name);

                if (!Directory.Exists(RealPath))
                {
                    LogClient.LogMessage($"Creating Directory {DInfo.Name}...");
                    CheckSendPatchMessage();
                    Directory.CreateDirectory(RealPath);
                }
                PatchDirectory(D);
            }


            if (File.Exists(DelFilePath))
            {
                DirectoryInfo RocketDIR = new DirectoryInfo(PathHelpers.RocketDirectory);
                string[] DelLines = File.ReadAllLines(DelFilePath);
                bool PatchSent = false;
                foreach (string _DelLine in DelLines)
                {
                    string DelLine = _DelLine;
                    DelLine = DelLine.Trim(' ');
                    if (DelLine.StartsWith("#") || DelLine == "") continue;
                    if (!PatchSent)
                    {
                        CheckSendPatchMessage();
                        PatchSent = true;
                        LogClient.LogMessage($"Running Delete Patches from PatchMod.Remlist.txt");
                    }
                    string Target = Path.Combine(PathHelpers.RocketDirectory, DelLine);
                    string LocalTarget = Path.Combine(PathHelpers.PatchModPatchDirectory, DelLine);
                    if (File.Exists(LocalTarget)) File.Delete(LocalTarget);
                    if (Directory.Exists(LocalTarget)) Directory.Delete(LocalTarget);

                    if (File.Exists(Target))
                    {
                        LogClient.LogMessage($"Deleting File {DelLine}");
                        File.Delete(Target);
                    }
                    else if (Directory.Exists(Target))
                    {
                        DirectoryInfo TDir = new DirectoryInfo(Target);
                        if (TDir.FullName.ToLower() != RocketDIR.FullName.ToLower())
                        {
                            LogClient.LogMessage($"Deleting Directory {DelLine}");
                            Directory.Delete(Target, true);
                        }
                        else
                        {
                            LogClient.LogMessage($"Cannot delete Rocket Directory; Directory Protected.");
                        }
                    }
                }
                if (PatchSent)
                {
                    File.WriteAllText(DelFilePath, "#Put paths to file/folders to delete in here.\n");
                }
            }

        }

        public static void PatchDirectory(string dir)
        {
            string Rel = PathHelpers.GetRelativePath(PathHelpers.PatchModPatchDirectory, dir);
            string Ref = PathHelpers.RocketDirectory + Rel;
            foreach (string F in Directory.GetFiles(dir))
            {
                CheckSendPatchMessage();
                FileInfo FInfo = new FileInfo(F);
                string RealPath = Path.Combine(Ref, FInfo.Name);
                LogClient.LogMessage($"Patching File {Rel + "\\" + FInfo.Name}...");
                if (File.Exists(RealPath)) File.Delete(RealPath);
                File.Move(F, RealPath);
            }
            foreach (string D in Directory.GetDirectories(dir))
            {
                FileInfo DInfo = new FileInfo(D);
                string RealPath = Path.Combine(Ref, DInfo.Name);
                if (!Directory.Exists(RealPath))
                {
                    CheckSendPatchMessage();
                    LogClient.LogMessage($"Creating Directory {Rel + "\\" + DInfo.Name}...");
                    Directory.CreateDirectory(RealPath);
                }
                PatchDirectory(D);
            }
        }
    }
}