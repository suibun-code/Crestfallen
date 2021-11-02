using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;

public class FileManager : MonoBehaviour
{
    string path;
    string copyPath;
    string fileName;
    public AudioSource audioSource;

    public List<String> fileNames;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenFileExplorer()
    {
        path = "C:/Users/" + Environment.UserName + "/AppData/Local/GatewayShift/";
        fileName = Path.GetFileName(path);
        copyPath = Application.dataPath + "/BeatTracks/" + fileName;

        if (Directory.Exists(path))
            StartCoroutine(LoadAudio());
    }

    public void PlayClip()
    {
        audioSource.Play();
    }

    IEnumerator LoadAudio()
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + path, AudioType.WAV);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(www.error);
        else
            audioSource.clip = DownloadHandlerAudioClip.GetContent(www);

        GetDirectoryInfo();
    }

    public void GetDirectoryInfo()
    {
        var info = new DirectoryInfo(Application.dataPath + "/BeatTracks");
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            Debug.Log(file.Name);
        }
    }

    public void Import()
    {
        var info = new DirectoryInfo(Application.dataPath + "/BeatTracks");
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            Debug.Log(file.Name);
        }

        // if (!Directory.Exists(copyPath))
        //     FileUtil.CopyFileOrDirectory(path, copyPath);
        // else
        //     Debug.Log("file exists");
    }
}
