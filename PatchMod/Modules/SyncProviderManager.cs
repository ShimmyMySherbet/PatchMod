using PatchMod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PatchMod.Modules
{
    public static class SyncProviderManager
    {
        public static SyncSource GetSource(string Name)
        {
            foreach(Type T in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (T == typeof(SyncSource)) continue;
                if (typeof(SyncSource).IsAssignableFrom(T))
                {
                    SyncSource Instance = (SyncSource)Activator.CreateInstance(T);
                    if (Instance.Name.ToLower() == Name.ToLower()) return Instance;
                }
            }
            return null;
        }
    }
}
