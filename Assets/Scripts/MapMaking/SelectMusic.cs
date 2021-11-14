using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;

public class SelectMusic : Singleton<SelectMusic>
{
    string tracksPath;
    string musicPath;
    string musicName;
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

    private new void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
        musicPath = tracksPath + musicName;
        Debug.Log(musicPath);
    }

    public void OpenAudioPath()
    {
        FileDialogPlugin.OpenExplorerWithPath("C:/Users/" + Environment.UserName + "/AppData/LocalLow/RaminAmiri/GatewayShift/tracks");
    }

    public void SelectSong()
    {
        musicPath = FileDialogPlugin.OpenFileDialog("Upload an audio file to use as your music.", "Import a *.mp3 file", "*.mp3");
        musicName = FileDialogPlugin.GetFileName();
    }

    public async void LoadMusic()
    {
        MapMakerManager.instance.musicPath = musicPath;
        MapMakerManager.instance.musicName = musicName;
        Debug.Log(MapMakerManager.instance.musicName);

        Text_fileName.SetText(musicName);

        Debug.Log("Loading...");
        playButton.GetComponent<Button>().interactable = false; //Disable the button
        playButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Loading..."); //Set the button's text to "Loading..."

        Text_fileName.color = Color.white;
        playButton.GetComponent<Button>().interactable = true; //Enable the button
        playButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Play"); //Set the button's text to "Play"

        /*If the file doesn't exist, 
        make the file name text color red, 
        and inform the user that the *.mp3 file is not valid*/
        if (!File.Exists(musicPath))
        {
            Text_fileName.color = Color.red;
            Text_fileName.SetText("\"" + musicName + "\"\n does not exist, is not the correct file type, or an invalid \"unofficial\" *.mp3.");
            Debug.Log("path doesnt exist");
            return;
        }

        audioSource.clip = await LoadFile.instance.LoadAudioFile(musicPath);
        //StartCoroutine(LoadFile.LoadAudioFile(audioSource.clip, musicPath));
    }

    public void PlayClip()
    {
        if (audioSource != null)
            audioSource.Play();
    }

    public void StopClip()
    {
        if (audioSource != null)
            audioSource.Stop();
    }

    public void RefreshDirectory()
    {
        if (files.Count > 0)
        {
            foreach (var file in files)
                Destroy(file);
        }

        //Get the names of files in the directory and add them into a string list
        var info = new DirectoryInfo(tracksPath);
        var fileInfo = info.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);

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
}
