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

    private bool doneSpinning = true;

    private string musicPath;
    private string musicName;
    private string tracksPath;

    public RectTransform rect_refresh;
    public RectTransform rect_title;
    public RectTransform rect_topMsg;

    public GameObject scrollView;
    public GameObject audioFileCell;

    public Button previewAudioButton;
    public Button stopPreviewButton;
    public Button nextButton;

    public List<GameObject> files;

    public AudioFileCell currentCell;

    private TextMeshProUGUI text_title;
    public TextMeshProUGUI text_fileName;
    public TextMeshProUGUI text_noFilesFound;

    private Color color_newTextTitle;
    public Color color_fileName;


    void OnEnable()
    {
        AudioFileCell.OnClick += SetCurrentCell;
    }

    void Start()
    {
        text_title = rect_title.GetChild(0).GetComponent<TextMeshProUGUI>();
        color_newTextTitle = new Color(text_title.color.r, text_title.color.g, text_title.color.b, 0f);

        StartCoroutine(AnimateUploadMusicUI());
        tracksPath = MapMakerManager.instance.TracksPath;
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
        FileDialogPlugin.OpenExplorerWithPath(MapMakerManager.instance.g_TracksPath);
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

        previewAudioButton.interactable = false; //Disable the button
        stopPreviewButton.interactable = false;
        nextButton.interactable = false;

        text_fileName.color = Color.white;
        text_fileName.SetText("Loading..."); //Set the button's text to when it's loading and disabled

        /*If the file doesn't exist, 
        make the file name text color red, 
        and inform the user that the *.mp3 file is not valid*/
        if (!File.Exists(musicPath))
        {
            text_fileName.color = Color.red;

            if (musicName == null)
                text_fileName.SetText("Please select a file to load.");
            else
                text_fileName.SetText("The file does not exist or is not the correct file type.");

            Debug.Log("path doesnt exist");
            return;
        }
        StartCoroutine(LoadAudioFile(musicPath));
    }

    public void RefreshDirectory()
    {
        if (doneSpinning)
            LeanTween.rotateAroundLocal(rect_refresh, Vector3.forward, -360f, 1f).setEaseOutCirc().setOnComplete(SetDoneSpinning);
        rect_refresh.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        doneSpinning = false;

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
            text_noFilesFound.enabled = false;
        else
            text_noFilesFound.enabled = true;

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

    private void SetDoneSpinning()
    {
        doneSpinning = true;
    }

    private IEnumerator AnimateUploadMusicUI()
    {
        yield return LeanTween.moveX(rect_title, 0f, 1f).setEaseOutCirc();

        while (text_title.color.a < 1f)
        {
            color_newTextTitle.a += 1f * Time.deltaTime;
            text_title.color = color_newTextTitle;
            yield return null;
        }

        yield return LeanTween.moveX(rect_topMsg, 110f, 1f).setEaseOutCirc();

        yield return new WaitForSeconds(1f);

        eventSystem.SetActive(true);
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
        {
            SongManager.instance.music.clip = DownloadHandlerAudioClip.GetContent(www);

            previewAudioButton.interactable = true;
            stopPreviewButton.interactable = true;
            nextButton.interactable = true;

            var playText = previewAudioButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var stopText = stopPreviewButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var nextText = nextButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            playText.alpha = 1f;
            stopText.alpha = 1f;
            nextText.alpha = 1f;

            text_fileName.color = color_fileName;
            text_fileName.SetText(musicName);
        }
    }

    public void OnEscape(InputValue value)
    {
        SceneManager.instance.ChangeScene("MainMenu");
    }
}
