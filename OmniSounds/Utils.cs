using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace OmniSounds;

public static class Utils
{
    static AudioClip FallbackClip = AudioClip.Create("ERROR", 44100, 1, 44100, false);
    static HashSet<string> validExtensions = new HashSet<string> { ".mp3", ".ogg", ".wav", ".flac", ".aiff", ".aif", ".mod", ".it", ".s3m", ".xm" };
    public static AudioClip LoadClipFromPath(string path)
    {
        // shamelessly stolen from soundreplacer because the code i already have to do this is an ienumerator and im too lazy to do that
        // unknown should be ok? ive done it before and nothing bad happened. not needing logic for this is a win in my book
        var request = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.UNKNOWN);
        var task = request.SendWebRequest();
            
        while (!task.isDone) { }

        if (request.result != UnityWebRequest.Result.Success)
        {
            return FallbackClip;
        }
        return DownloadHandlerAudioClip.GetContent(request);
    }

    public static bool IsAudioFormatSupported(string file)
    {
        return validExtensions.Contains(Path.GetExtension(file).ToLower());
    }
}