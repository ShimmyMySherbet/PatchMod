using FluentFTP;
using PatchMod.Models;
using PatchMod.Modules;
using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PatchMod.SyncProviders
{
    /// <summary>
    /// Experimental, not fully completed yet.
    /// </summary>
    public class FTPSyncSource : SyncSource
    {
        public override string Name => "FTP";

        public FtpClient Client;

        public override void Init()
        {
            Client = new FtpClient((string)PatchMod.Config["FTPHost"], (int)PatchMod.Config["FTPPort", typeof(int)], new NetworkCredential((string)PatchMod.Config["FTPUsername"], (string)PatchMod.Config["FTPPassword"]));
            if (Client.AutoConnect() != null)
            {
                LogClient.LogMessage($"FTP Connected.");
            } else
            {
                LogClient.LogMessage($"Unable to connect to remote host via FTP.");
            }
        }

        public override void Shutdown()
        {
            Client.Dispose();
        }

        public override SyncDirectory[] GetDirectories(string dir)
        {
            string Local = Path.Combine(Source, dir);
            Client.SetWorkingDirectory(Local);
            List<SyncDirectory> Dirs = new List<SyncDirectory>();
            foreach(var e in Client.GetListing())
            {
                if (e.Type == FtpFileSystemObjectType.Directory) Dirs.Add(new SyncDirectory() { Name = e.Name, Path = e.FullName, Source = this});
            }
            return Dirs.ToArray();
        }

        public override SyncFile[] GetFiles(string dir)
        {
            string Local = Path.Combine(Source, dir);
            Client.SetWorkingDirectory(Local);
            List<SyncFile> Files = new List<SyncFile>();
            foreach (var e in Client.GetListing())
            {
                if (e.Type == FtpFileSystemObjectType.File) Files.Add(new SyncFile() { Name = e.Name, Path = e.FullName, Source = this });
            }
            return Files.ToArray();
        }

        public override Stream ReadFile(string path)
        {
            string Local = Path.Combine(Source, path);
            MemoryStream MemS = new MemoryStream();
            if(!Client.Download(MemS, Local))
            {
                LogClient.LogMessage($"Failed to download file {path}");
            }
            MemS.Seek(0, SeekOrigin.Begin);
            return MemS;
        }

        public override bool CompareFiles(string path, string localpath)
        {
            string Remote = "/" + Path.Combine(Source, path.Trim('/', '\\'));
            var result = Client.CompareFile(localpath, Remote, FtpCompareOption.Checksum);
            if (result == FtpCompareResult.FileNotExisting) return true;
            if (result == FtpCompareResult.ChecksumNotSupported)
            {
                result = Client.CompareFile(localpath, Remote, FtpCompareOption.Size);
            }
            return result == FtpCompareResult.Equal;
        }
    }
}
