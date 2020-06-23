using System.IO;

namespace PatchMod.Models
{
    public abstract class SyncSource
    {
        public string Source;
        public bool FilesChanged = false;
        public int NewFiles = 0;
        public abstract string Name { get; }

        public abstract SyncFile[] GetFiles(string dir);

        public abstract SyncDirectory[] GetDirectories(string dir);

        public abstract Stream ReadFile(string path);

        public virtual bool CompareFiles(string path, string localpath)
        {
            return false;
        }
        public virtual void Init() { }
        public virtual void Shutdown() { }
    }
}