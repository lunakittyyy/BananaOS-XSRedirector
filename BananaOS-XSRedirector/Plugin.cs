using BepInEx;

namespace BananaOS_XSRedirector
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public Plugin()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }
    }
}