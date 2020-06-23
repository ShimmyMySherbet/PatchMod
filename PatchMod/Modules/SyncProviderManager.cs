using PatchMod.Models;
using System;
using System.Reflection;

namespace PatchMod.Modules
{
    public static class SyncProviderManager
    {
        public static SyncSource GetSource(string Name)
        {
            foreach (Type T in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (T == typeof(SyncSource)) continue;
                if (typeof(SyncSource).IsAssignableFrom(T))
                {
                    SyncSource Instance = (SyncSource)Activator.CreateInstance(T);
                    if (Instance.Name.ToLower() == Name.ToLower()) return Instance;
                }
            }
            foreach(Assembly Plugin in LibManager.AvailablePlugins)
            {
                foreach(Type T in Plugin.GetTypes())
                {
                    if (typeof(SyncSource).IsAssignableFrom(T))
                    {
                        SyncSource Instance = (SyncSource)Activator.CreateInstance(T);
                        if (Instance.Name.ToLower() == Name.ToLower()) return Instance;
                    }
                }
            }
            return null;
        }
    }
}