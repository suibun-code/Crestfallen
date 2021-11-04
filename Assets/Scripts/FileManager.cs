using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;

public class FileManager : MonoBehaviour
{
    string path;
    string filePath;
    string fileName;
    public AudioSource audioSource;
    public AudioFileCell currentCell;
    public List<GameObject> files;
    public GameObject scrollView;
    public GameObject audioFileCell;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/songs/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/songs/");

        path = Application.persistentDataPath + "/songs/";
        RefreshDirectory();
    }

    public void OpenFileExplorer()
    {
        //Set the path to store songs
        filePath = path + fileName;

        if (Directory.Exists(path))
            StartCoroutine(LoadAudio());
    }

    public void PlayClip()
    {
        audioSource.Play();
    }

    //use to allow user to select which song theyd like to use
    IEnumerator LoadAudio()
    {
        //Load audio from the chosen .wav file
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(www.error);
        else
            audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
    }

    public void RefreshDirectory()
    {
        if (files.Count > 0)
        {
            foreach (var file in files)
                Destroy(file);
        }

        //Get the names of files in the directory and add them into a string list
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles("*.mp3", SearchOption.AllDirectories);

        foreach (var file in fileInfo)
        {
            var cell = Instantiate(audioFileCell);
            files.Add(cell);
            cell.transform.SetParent(scrollView.GetComponent<ScrollRect>().content.transform);
            cell.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(file.Name);
            fileName = file.Name;
        }
    }
}
