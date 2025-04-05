using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using IPA.Utilities;
using UnityEngine;
using static OmniSounds.Utils;

namespace OmniSounds;

[HarmonyPatch(typeof(EffectPoolsManualInstaller), "ManualInstallBindings")]
public class YaFuckedUpPatcher
{
    public static void Prefix(EffectPoolsManualInstaller __instance)
    {
        Plugin.Log.Info("Enumerating sounds for bad slice");
        var files = Directory.GetFiles($"{UnityGame.UserDataPath}/OmniSounds/BadSliceSounds");
        if (files.Length != 0)
        {
            var clips = new List<AudioClip>();
            foreach (var file in files)
            {
                if (IsAudioFormatSupported(file))
                {
                    Plugin.Log.Info("Adding bad slice sound at " + file);
                    clips.Add(LoadClipFromPath(file));
                }
            }
            Plugin.Log.Info("Cloning bad slice prefab");
            var newObj = Object.Instantiate(__instance._noteCutSoundEffectPrefab);
            Plugin.Log.Info("Setting bad slice audio clips here!");
            newObj._badCutSoundEffectAudioClips = clips.ToArray();
            Plugin.Log.Info("Setting bad slice instance to its clone");
            __instance._noteCutSoundEffectPrefab = newObj;
        }
    }
}