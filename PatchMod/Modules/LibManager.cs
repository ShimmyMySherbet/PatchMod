using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PatchMod.Modules
{
    public static class LibManager
    {
        public static List<Assembly> AvailablePlugins = new List<Assembly>();
        public static void LoadDeps()
        {
            foreach (string dep in Directory.GetFiles(PathHelpers.PatchModDepDirectory, "*.dll"))
            {
                Assembly.LoadFrom(dep);
            }
        }

        public static void LoadPlugins()
        {
            foreach (string Plug in Directory.GetFiles(PathHelpers.PatchModPluginsDirectory))
            {
                AvailablePlugins.Add(Assembly.LoadFrom(Plug));
            }
        }
    }
}
