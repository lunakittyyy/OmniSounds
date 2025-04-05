using System.IO;
using System.Reflection;
using HarmonyLib;
using IPA;
using IPA.Loader;
using IPA.Utilities;
using IpaLogger = IPA.Logging.Logger;

namespace OmniSounds;

[Plugin(RuntimeOptions.DynamicInit)]
[NoEnableDisable]
internal class Plugin
{
    public static IpaLogger Log { get; private set; } = null!;

    private Harmony harmony;
    private Assembly executingAssembly = Assembly.GetExecutingAssembly();
    
    [Init]
    public Plugin(IpaLogger ipaLogger, PluginMetadata pluginMetadata)
    {
        Log = ipaLogger;
        Log.Info($"{pluginMetadata.Name} {pluginMetadata.HVersion} initialized.");
        harmony = new Harmony(pluginMetadata.Id);
    }

    [OnStart]
    public void OnApplicationStart()
    {
        Log.Info($"Creating bad slice sound directory");
        if (!Directory.Exists($"{UnityGame.UserDataPath}/OmniSounds/BadSliceSounds"))
            Directory.CreateDirectory($"{UnityGame.UserDataPath}/OmniSounds/BadSliceSounds");
        Log.Info($"Creating regular slice sound directory");
        if (!Directory.Exists($"{UnityGame.UserDataPath}/OmniSounds/SliceSounds"))
            Directory.CreateDirectory($"{UnityGame.UserDataPath}/OmniSounds/SliceSounds");
        harmony.PatchAll(executingAssembly);
    }

    [OnExit]
    public void OnApplicationQuit() => harmony.UnpatchSelf();
}