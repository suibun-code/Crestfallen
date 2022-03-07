using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.InputSystem;

public class SelectMusic : Singleton<SelectMusic>
{
    public GameObject eventSystem;

    private string musicPath;
    private string musicName;
    private string tracksPath;
    private string songsPath;

    public GameObject scrollView;
    public GameObject audioFileCell;

    public Button previewAudioButton;
    public Button stopPreviewButton;
    public Button nextButton;

    public List<GameObject> files;

    public AudioFileCell currentCell;

    public TextMeshProUGUI fileSelected;
    public TextMeshProUGUI foundFilesText;

    public Color fileNameColor;

    void OnEnable()
    {
        AudioFileCell.OnClick += SetCurrentCell;
    }

    void Start()
    {
        tracksPath = MapMakerManager.instance.TracksPath;
        songsPath = MapMakerManager.instance.g_SongsPath;
        RefreshDirectory();
    }

    void SetCurrentCell(AudioFileCell audioFileCell)
    {
        currentCell = audioFileCell;
        musicName = currentCell.fileName;
        musicPath = System.IO.Path.Combine(tracksPath, musicName);
        LoadMusic();
        Debug.Log(musicPath);
    }

    public void OpenAudioPath()
    {
        FileDialogPlugin.OpenExplorerWithPath(MapMakerManager.instance.g_SongsPath);
    }

    public void SelectSong()
    {
        musicPath = FileDialogPlugin.OpenFileDialog("Upload an audio file to use as your music.", "Import a *.mp3 file", "*.mp3");
        musicName = FileDialogPlugin.GetFileName();
        LoadMusic();
    }

    public void LoadMusic()
    {
        MapMakerManager.instance.musicPath = musicPath;
        MapMakerManager.instance.musicName = musicName;

        //Disable assosciated buttons
        previewAudioButton.interactable = false;
        stopPreviewButton.interactable = false;
        nextButton.interactable = false;

        //Set the fileSelected text and color to when it's loading
        fileSelected.color = Color.white;
        fileSelected.SetText("Loading...");

        /*If the file doesn't exist, 
        make the file name text color red, 
        and inform the user that the *.mp3 file is not valid*/
        if (!File.Exists(musicPath))
        {
            fileSelected.color = Color.red;

            if (musicName == null)
                fileSelected.SetText("Please select a file to load.");
            else
                fileSelected.SetText("The file does not exist or is not the correct file type.");

            Debug.Log("path doesnt exist");
            return;
        }
        StartCoroutine(IE_LoadAudioFile(musicPath));
    }

    private IEnumerator IE_LoadAudioFile(string path) // Loads *.mp3's
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
            //Get the clip and assign it to the song manager's AudioSource
            SongManager.instance.music.clip = DownloadHandlerAudioClip.GetContent(www);

            //Make buttons associated with select music to interactable once something is selected
            previewAudioButton.interactable = true;
            stopPreviewButton.interactable = true;
            nextButton.interactable = true;

            //Reset alphas of play, stop, and next
            Image playText = previewAudioButton.gameObject.transform.GetChild(0).GetComponent<Image>();
            Image stopText = stopPreviewButton.gameObject.transform.GetChild(0).GetComponent<Image>();
            Image nextText = nextButton.gameObject.transform.GetChild(0).GetComponent<Image>();
            playText.color = new Color(playText.color.r, playText.color.g, playText.color.b, 1.0f);
            stopText.color = new Color(stopText.color.r, stopText.color.g, stopText.color.b, 1.0f);
            nextText.color = new Color(nextText.color.r, nextText.color.g, nextText.color.b, 1.0f);

            //Change color of the file selected text (music selected) to the specificed color (chosen in editor) and set the text to the file name
            fileSelected.color = fileNameColor;
            fileSelected.SetText(musicName);
        }
    }

    public void RefreshDirectory()
    {
        if (CreateTrackTweens.instance.IE_animateAudioFileCells != null)
        {
            StopCoroutine(CreateTrackTweens.instance.IE_animateAudioFileCells);
        }

        //If audio file cells already exist, destroy them so the list can be properly updated
        if (files.Count > 0)
        {
            foreach (var file in files)
                Destroy(file);
        }
        files.Clear();

        //Get the names of *.mp3 files in the tracks root directory and add them into a string list
        DirectoryInfo info = new DirectoryInfo(songsPath);
        FileInfo[] fileInfo = info.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);

        //Enable or disable the "no files found" text depending if files are found in the directory
        if (fileInfo.Length > 0)
            foundFilesText.enabled = false;
        else
            foundFilesText.enabled = true;

        LoadAudioFileCells(fileInfo);
    }

    private void LoadAudioFileCells(FileInfo[] fileInfo)
    {
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

        CreateTrackTweens.instance.IE_animateAudioFileCells = StartCoroutine(CreateTrackTweens.instance.IE_AnimateAudioFileCells(files));
    }

    public void OnEscape(InputValue value)
    {
        SceneManager.instance.ChangeScene("MainMenu");
    }
}
