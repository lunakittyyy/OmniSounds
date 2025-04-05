using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using IPA.Utilities;
using UnityEngine;
using static OmniSounds.Utils;

namespace OmniSounds;

[HarmonyPatch(typeof(NoteCutSoundEffectManager), "Start", MethodType.Normal)]
public static class SlicePatcher
{
    public static void Prefix(NoteCutSoundEffectManager __instance)
    {
        Plugin.Log.Info("Enumerating sounds for good slice");
        var files = Directory.GetFiles($"{UnityGame.UserDataPath}/OmniSounds/SliceSounds");
        if (files.Length != 0)
        {
            var clips = new List<AudioClip>();
            foreach (var file in files)
            {
                if (IsAudioFormatSupported(file))
                {
                    Plugin.Log.Info("Adding good slice sound at " + file);
                    clips.Add(LoadClipFromPath(file));
                }
            }
            Plugin.Log.Info("Setting good slice audio clips here!");
            __instance._shortCutEffectsAudioClips = clips.ToArray();
            __instance._longCutEffectsAudioClips = clips.ToArray();
        }
    }
}