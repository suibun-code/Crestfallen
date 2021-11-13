using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class LoadFile : Singleton<LoadFile>
{
    public async Task<AudioClip> LoadAudioFile(string path) // Loads *.mp3's
    {
        //Load audio from the chosen *.mp3 file
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        /*If there was an error loading the audio file, 
        log the error. Otherwise, set it to the audioSource*/
        if (www.result == UnityWebRequest.Result.Success)
            return DownloadHandlerAudioClip.GetContent(www);
        else
            return null;
    }

    public async Task<Texture> LoadImage(string path) // Loads *.png's and *.jpg's
    {
        //Load texture from the chosen image file
        using UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        /*If there was an error loading the texture, 
        log the error. Otherwise, set it to the avatar*/
        if (www.result == UnityWebRequest.Result.Success)
            return DownloadHandlerTexture.GetContent(www);
        else
            return null;
    }
}
