using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;

public class MusicFileManager : MonoBehaviour
{
    string path;
    string savePath;
    string filePath;
    string fileName;
    public AudioSource audioSource;
    public AudioFileCell currentCell;
    public List<GameObject> files;
    public GameObject scrollView;
    public GameObject audioFileCell;
    public GameObject playButton;

    public TextMeshProUGUI Text_fileName;

    void OnEnable()
    {
        AudioFileCell.OnClick += SetCurrentCell;
    }

    void SetCurrentCell(AudioFileCell audioFileCell)
    {
        currentCell = audioFileCell;
        fileName = currentCell.fileName;
        filePath = path + fileName;
    }

    private void Awake()
    {
        //Create appropriate directories
        if (!Directory.Exists(Application.persistentDataPath + "/songs/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/songs/");
        path = Application.persistentDataPath + "/songs/";

        if (!Directory.Exists(Application.persistentDataPath + "/tracks/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/tracks/");
        savePath = Application.persistentDataPath + "/tracks/";

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        RefreshDirectory();
    }

    public void OnOpenFileDialog()
    {
        filePath = FileDialogPlugin.OpenFileDialog();
        fileName = FileDialogPlugin.GetFileName();
    }

    public void SelectAudioFile()
    {
        Text_fileName.SetText(fileName);

        Debug.Log("Loading...");
        playButton.GetComponent<Button>().interactable = false; //Disable the button
        playButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Loading..."); //Set the button's text to "Loading..."

        StartCoroutine(LoadAudio());
    }

    public void PlayClip()
    {
        audioSource.Play();
    }

    //use to allow user to select which song theyd like to use
    IEnumerator LoadAudio()
    {
        if (!File.Exists(filePath))
        {
            Text_fileName.color = Color.red;
            Text_fileName.SetText("\"" + fileName + "\"\n does not exist, is not the correct file type, or an unsupported \"unofficial\" *.mp3.");
            Debug.Log("path doesnt exist");
            yield break;
        }

        Text_fileName.color = Color.white;

        //Load audio from the chosen .wav file
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG);

        yield return www.SendWebRequest();

        Debug.Log("Done loading");
        playButton.GetComponent<Button>().interactable = true; //Enable the button
        playButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Play"); //Set the button's text to "Play"

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
            //Set the values for the audio file cell
            var cell = Instantiate(audioFileCell);
            files.Add(cell); //Ad cell to the files list to be managed
            cell.GetComponent<AudioFileCell>().fileName = file.Name; //Set the fileName variable in the cell to equal the file's name
            cell.transform.SetParent(scrollView.GetComponent<ScrollRect>().content.transform, false); //Attach cell the the scrollview to be organized in
            cell.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(file.Name); //Set the text of the cell to be the file name
        }
    }

    public void Save(Beatmap beatmap, string musicPath)
    {
        //Save audio file to path
        File.Copy(filePath, savePath + fileName);

        
        var json = JsonUtility.ToJson(beatmap);
        System.IO.File.WriteAllText(savePath + beatmap.name + ".gst", json);
    }
}
