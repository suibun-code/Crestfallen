using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class SelectMusic : Singleton<SelectMusic>
{
    string tracksPath;
    string musicPath;
    string musicName;
    public AudioFileCell currentCell;
    public List<GameObject> files;
    public GameObject scrollView;
    public GameObject audioFileCell;
    public GameObject playButton;
    public TextMeshProUGUI text_fileName;
    public TextMeshProUGUI text_noFilesFound;

    void OnEnable()
    {
        AudioFileCell.OnClick += SetCurrentCell;
    }

    void Start()
    {
        tracksPath = MapMakerManager.instance.TracksPath;
        RefreshDirectory();
    }

    void SetCurrentCell(AudioFileCell audioFileCell)
    {
        currentCell = audioFileCell;
        musicName = currentCell.fileName;
        musicPath = System.IO.Path.Combine(tracksPath, musicName);
        Debug.Log(musicPath);
    }

    public void OpenAudioPath()
    {
        FileDialogPlugin.OpenExplorerWithPath(MapMakerManager.instance.g_TracksPath);
    }

    public void SelectSong()
    {
        musicPath = FileDialogPlugin.OpenFileDialog("Upload an audio file to use as your music.", "Import a *.mp3 file", "*.mp3");
        musicName = FileDialogPlugin.GetFileName();
    }

    public void LoadMusic()
    {
        MapMakerManager.instance.musicPath = musicPath;
        MapMakerManager.instance.musicName = musicName;
        Debug.Log(MapMakerManager.instance.musicName);

        text_fileName.SetText(musicName);

        playButton.GetComponent<Button>().interactable = false; //Disable the button
        playButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Loading..."); //Set the button's text to "Loading..."

        text_fileName.color = Color.white;
        playButton.GetComponent<Button>().interactable = true; //Enable the button
        playButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Play"); //Set the button's text to "Play"

        /*If the file doesn't exist, 
        make the file name text color red, 
        and inform the user that the *.mp3 file is not valid*/
        if (!File.Exists(musicPath))
        {
            text_fileName.color = Color.red;
            text_fileName.SetText("\"" + musicName + "\"\n does not exist or is not the correct file type.");
            Debug.Log("path doesnt exist");
            return;
        }

        StartCoroutine(LoadAudioFile(musicPath));
    }

    public void RefreshDirectory()
    {
        //If audio file cells exist, destroy them so the list can be properly updated
        if (files.Count > 0)
        {
            foreach (var file in files)
                Destroy(file);
        }

        //Get the names of *.mp3 files in the tracks root directory and add them into a string list
        var info = new DirectoryInfo(tracksPath);
        var fileInfo = info.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);
        
        //Enable or disable the "no files found" text depending if files are found in the directory
        if (fileInfo.Length > 0)
        {
            text_noFilesFound.enabled = false;
            Debug.Log("disabled");
        }
        else
        {
            text_noFilesFound.enabled = true;
            Debug.Log("enabled");
        }

        //Browse through all found *.mp3 files and create audio file cells for each of them
        foreach (var file in fileInfo)
        {
            var cell = Instantiate(audioFileCell);
            files.Add(cell);

            //Set the values for the audio file cell
            cell.GetComponent<AudioFileCell>().fileName = file.Name; //Set the fileName variable in the cell to equal the file's name
            cell.transform.SetParent(scrollView.GetComponent<ScrollRect>().content.transform, false); //Attach cell to the scrollview to be parented
            cell.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(file.Name); //Set the text of the cell to be the name of the file
        }
    }

    public IEnumerator LoadAudioFile(string path) // Loads *.mp3's
    {
        //Load audio from the chosen *.mp3 file
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
        yield return www.SendWebRequest();

        /*If there was an error loading the audio file, 
        log the error. Otherwise, set it to the audioSource*/
        if (www.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(www.error);
        else
            SongManager.instance.music.clip = DownloadHandlerAudioClip.GetContent(www);
    }
}
