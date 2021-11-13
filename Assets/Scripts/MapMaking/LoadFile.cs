using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadFile : MonoBehaviour
{
    public static IEnumerator LoadAudioFile(AudioClip clip, string path) // Loads *.mp3's
    {
        //Load audio from the chosen *.mp3 file
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
        yield return www.SendWebRequest();

        /*If there was an error loading the audio file, 
        log the error. Otherwise, set it to the audioSource*/
        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(www.error);
        else
        {
            clip = DownloadHandlerAudioClip.GetContent(www);
        }
    }

    public static IEnumerator LoadImage(Texture texture, string path) // Loads *.png's and *.jpg's
    {
        //Load texture from the chosen image file
        using UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        yield return www.SendWebRequest();

        /*If there was an error loading the texture, 
        log the error. Otherwise, set it to the avatar*/
        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(www.error);
        else
            texture = DownloadHandlerTexture.GetContent(www);
    }
}
