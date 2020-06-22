using PatchMod.Models;
using PatchMod.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace PatchMod.SyncProviders
{
    public class LocalSyncSource : SyncSource
    {
        public override string Name => "Local";

        public override SyncDirectory[] GetDirectories(string _Dir)
        {
            string dir = _Dir.TrimStart('\\', '/');
            string LPath = Path.Combine(Source, dir);
            List<SyncDirectory> SDirs = new List<SyncDirectory>();
            foreach (string LDir in Directory.GetDirectories(LPath))
            {
                DirectoryInfo LDInfo = new DirectoryInfo(LDir);
                string RelPath = PathHelpers.GetRelativePath(Source, LDInfo.FullName).TrimStart('\\', '/');
                SDirs.Add(new SyncDirectory() { Name = LDInfo.Name, Path = RelPath, Source = this });
            }
            return SDirs.ToArray();
        }

        public override SyncFile[] GetFiles(string _Dir)
        {
            string dir = _Dir.TrimStart('\\', '/');
            string LPath = Path.Combine(Source, dir);
            List<SyncFile> SFiles = new List<SyncFile>();
            foreach (string LFile in Directory.GetFiles(LPath))
            {
                FileInfo LFileInfo = new FileInfo(LFile);
                string RelPath = PathHelpers.GetRelativePath(Source, LFileInfo.FullName).TrimStart('\\', '/');

                SFiles.Add(new SyncFile() { Name = LFileInfo.Name, Path = RelPath, Source = this });
            }
            return SFiles.ToArray();
        }

        public override Stream ReadFile(string path)
        {
            string RealPath = Path.Combine(Source, path);
            return new FileStream(RealPath, FileMode.Open, FileAccess.Read);
        }

        public override bool CompareFiles(string path, string localpath)
        {
            string RealPath = Path.Combine(Source, path);
            FileInfo LInf = new FileInfo(RealPath);
            FileInfo RInf = new FileInfo(localpath);
            if (LInf.Length != RInf.Length) return false;
            return (ComputeHash(RealPath) == ComputeHash(localpath));
        }

        private string ComputeHash(string Path)
        {
            SHA256 SHA = SHA256.Create();
            using (FileStream LocalStream = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                return Convert.ToBase64String(SHA.ComputeHash(LocalStream));
            }
        }
    }
}