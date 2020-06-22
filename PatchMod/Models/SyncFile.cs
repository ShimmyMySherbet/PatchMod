using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchMod.Models
{
    public class SyncFile
    {
        public SyncSource Source;
        public string Path;
        public string Name;
        public Stream GetStream()
        {
            return Source.ReadFile(Path);
        }
    }
}
