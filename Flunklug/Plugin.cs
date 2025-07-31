using System;
using BepInEx;
using BepInEx.Configuration;
using Flunklug.Behaviours;
using GorillaLocomotion;

namespace Flunklug
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [BepInDependency("dev.gorillainfowatch")]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> SpawnOnLoad;

        public void Awake()
        {
            SpawnOnLoad = Config.Bind(PluginInfo.Name, "Spawn On Load", false, "Whether a flunklug is spawned when the mod is loaded");
            GorillaTagger.OnPlayerSpawned(() => GTPlayer.Instance.gameObject.AddComponent<FlunkController>());
        }
    }
}
