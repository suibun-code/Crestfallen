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
        //Set the path to store songs
        path = Application.persistentDataPath;
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
        //Load audio from the given .wav file
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
        //Get the names of files in the directory and add them into a string list
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles(".", SearchOption.AllDirectories);
        foreach (var file in fileInfo)
        {
            fileNames.Add(file.Name);
            Debug.Log(file.Name);
        }
    }

    public void Import()
    {
        // System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog()
        // {
        //     FileName = "Select a .wav or .ogg file",
        //     Filter = "*.wav | *.ogg",
        //     Title = "Open .wav or .ogg file"
        // };

        // if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        // {
        //     Debug.Log("POG");
        // }
    }
}
