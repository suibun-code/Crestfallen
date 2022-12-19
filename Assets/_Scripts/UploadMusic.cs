using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.InputSystem;

public class UploadMusic : MonoBehaviour
{
    [SerializeField] private GameObject scrollView;
    [SerializeField] private GameObject audioFileCell;

    private AudioSource _audioSource;

    private string _musicPath;
    private string _musicName;
    private string _songsPath;

    public Button previewAudioButton;
    public Button stopPreviewButton;
    public Button nextButton;

    public List<GameObject> files;

    public AudioFileCell currentCell;

    public TextMeshProUGUI fileSelected;
    public TextMeshProUGUI foundFilesText;

    public Color fileNameColor;
    public Color fileLoadingColor;
    public Color fileNotFoundColor;

    void OnEnable()
    {
        AudioFileCell.OnClick += SetCurrentCell;
    }

    private void OnDisable()
    {
        AudioFileCell.OnClick -= SetCurrentCell;
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _songsPath = MapMakerManager.instance.g_SongsPath;
        RefreshDirectory();
    }

    void SetCurrentCell(AudioFileCell audioFileCell)
    {
        currentCell = audioFileCell;
        _musicName = currentCell.fileName;
        _musicPath = System.IO.Path.Combine(_songsPath, _musicName);
        
        LoadMusic();

        SongManager.instance.music.clip.name = _musicName;
        Debug.Log(SongManager.instance.music.clip.name);
    }

    public void OpenAudioPath()
    {
        StartCoroutine(IE_OpenAudioPath());
    }

    public void SelectSong()
    {
        StartCoroutine(IE_SelectSong());
    }

    public void LoadMusic()
    {
        MapMakerManager.instance.musicPath = _musicPath;
        MapMakerManager.instance.musicName = _musicName;

        //Disable assosciated buttons
        previewAudioButton.interactable = false;
        stopPreviewButton.interactable = false;
        nextButton.interactable = false;

        //Set the fileSelected text and color to when it's loading
        fileSelected.color = fileLoadingColor;
        fileSelected.SetText("Loading...");

        /*If the file doesn't exist, 
        make the file name text color red, 
        and inform the user that the *.mp3 file is not valid*/
        if (!File.Exists(_musicPath))
        {
            fileSelected.color = fileNotFoundColor;

            if (_musicName == null)
                fileSelected.SetText("Please select a file to load.");
            else
                fileSelected.SetText("The file does not exist or is not the correct file type.");

            Debug.Log("path doesnt exist");
            return;
        }
        StartCoroutine(IE_LoadAudioFile(_musicPath));
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
            SongManager.instance.SetMusic(DownloadHandlerAudioClip.GetContent(www));

            //Make buttons associated with select music to interactable once something is selected
            previewAudioButton.interactable = true;
            stopPreviewButton.interactable = true;
            nextButton.interactable = true;

            //Reset alphas of play, stop, and next
            Image playImage = previewAudioButton.gameObject.transform.GetChild(0).GetComponent<Image>();
            Image stopImage = stopPreviewButton.gameObject.transform.GetChild(0).GetComponent<Image>();
            playImage.color = new Color(playImage.color.r, playImage.color.g, playImage.color.b, 1.0f);
            stopImage.color = new Color(stopImage.color.r, stopImage.color.g, stopImage.color.b, 1.0f);

            //Change color of the file selected text (music selected) to the specificed color (chosen in editor) and set the text to the file name
            fileSelected.color = fileNameColor;
            fileSelected.SetText(_musicName);
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
        DirectoryInfo info = new DirectoryInfo(_songsPath);
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
            cell.GetComponent<Button>().onClick.AddListener(PlayButtonAudio); //Add onClick to a function that plays the button sound, so every cell doesn't need its own AudioSource.
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
        CreateTrackTweens.instance.StopAnimatingUI();
        MenuManager.instance.SwitchMenu("MainMenu");
    }

    public void PlayButtonAudio() //Add onClick to a function that plays the button sound, so every AudioFileCell doesn't need its own AudioSource.
    {
        _audioSource.Play();
    }

    IEnumerator IE_SelectSong()
    {
        yield return StartCoroutine(IE_ButtonPause());

        _musicPath = FileDialogPlugin.OpenFileDialog("Upload an audio file to use as your music.", "Import a *.mp3 file", "*.mp3");
        _musicName = FileDialogPlugin.GetFileName();
        LoadMusic();
    }

    IEnumerator IE_OpenAudioPath()
    {
        yield return StartCoroutine(IE_ButtonPause());

        FileDialogPlugin.OpenExplorerWithPath(MapMakerManager.instance.g_SongsPath);
    }

    IEnumerator IE_ButtonPause()
    {
        yield return new WaitForSeconds(0.35f);
    }
}
