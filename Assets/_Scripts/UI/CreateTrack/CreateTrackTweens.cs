using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTrackTweens : Singleton<CreateTrackTweens>
{
    private void Start()
    {
        titlePosUM = titleUM.transform.position;
        topMessagePosUM = topMessageUM.transform.position;
        mainMessagePosUM = mainMessageUM.transform.position;
        oneTextPosUM = oneTextUM.transform.position;

        topMessagePosBI = topMessageBI.transform.position;
        og_mainMessagePosBI = mainMessageBI.transform.position;
        og_oneTextPosBI = oneTextBI.transform.position;

        titleTextUM = titleUM.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void StopAnimatingUI()
    {
        if (beatmapInfoUI != null)
            StopCoroutine(beatmapInfoUI);

        if (uploadMusicUI != null)
            StopCoroutine(uploadMusicUI);
    }

    #region SelectMusic
    [Space]
    [Header("Upload Music")]

    public GameObject eventSystem;

    private TextMeshProUGUI titleTextUM;
    private Color newTitleColorUM;

    private Vector3 titlePosUM;
    private Vector3 topMessagePosUM;
    private Vector3 mainMessagePosUM;
    private Vector3 oneTextPosUM;
    public RectTransform titleUM;
    public RectTransform topMessageUM;
    public RectTransform mainMessageUM;
    public RectTransform oneTextUM;

    public RectTransform refreshRect;
    private bool doneSpinning = true;

    public Coroutine IE_animateAudioFileCells;
    public Coroutine uploadMusicUI;

    public void AnimateUploadMusicUI()
    {
        //Reset all elements to original position
        titleUM.transform.position = titlePosUM;
        topMessageUM.transform.position = topMessagePosUM;
        mainMessageUM.transform.position = mainMessagePosUM;
        oneTextUM.transform.position = oneTextPosUM;

        newTitleColorUM = new Color(titleTextUM.color.r, titleTextUM.color.g, titleTextUM.color.b, 0f);
        titleTextUM.color = newTitleColorUM;

        uploadMusicUI = StartCoroutine(IE_AnimateUploadMusicUI());
    }

    private IEnumerator IE_AnimateUploadMusicUI()
    {
        eventSystem.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        yield return LeanTween.moveX(titleUM, 0f, 1f).setEaseOutCirc();

        while (titleTextUM.color.a < 1f)
        {
            newTitleColorUM.a += 0.75f * Time.deltaTime;
            titleTextUM.color = newTitleColorUM;
            yield return null;
        }

        yield return LeanTween.moveX(topMessageUM, -110f, 1f).setEaseOutCirc();
        yield return new WaitForSeconds(1f);
        yield return LeanTween.moveY(oneTextUM, 50f, 0.25f).setEaseOutCirc();
        yield return new WaitForSeconds(0.15f);
        yield return LeanTween.moveY(mainMessageUM, 20f, 0.5f).setEaseOutCirc();
        yield return new WaitForSeconds(0.75f);

        eventSystem.SetActive(true);
    }

    public IEnumerator IE_AnimateAudioFileCells(List<GameObject> files)
    {
        for (int i = 0; i < files.Count; i++)
        {
            if (files[i] == null)
                yield break;

            yield return new WaitForSeconds(0.25f);
            StartCoroutine(IE_IncreaseAudioCellOpacity(files, i));

            yield return null;
        }
    }

    public IEnumerator IE_IncreaseAudioCellOpacity(List<GameObject> files, int i)
    {
        Color audioFileCellBGColor = new Color(1f, 1f, 1f, 0f);

        while (files[i].GetComponent<Image>().color.a < 1f)
        {
            audioFileCellBGColor.a += 0.5f * Time.deltaTime;
            files[i].GetComponent<Image>().color = audioFileCellBGColor;
            files[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().alpha = audioFileCellBGColor.a;
            yield return null;
        }
    }

    public void SpinRefreshIcon()
    {
        if (doneSpinning)
            LeanTween.rotateAroundLocal(refreshRect, Vector3.forward, -360f, 1f).setEaseOutCirc().setOnComplete(SetDoneSpinning);
        refreshRect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        refreshRect.parent.gameObject.GetComponent<Button>().interactable = false;
        doneSpinning = false;
    }

    private void SetDoneSpinning()
    {
        refreshRect.parent.gameObject.GetComponent<Button>().interactable = true;
        doneSpinning = true;
    }

    #endregion SelectMusic

    #region BeatmapInfo
    [Space]
    [Header("Beatmap Info")]

    public Transform inputFieldsParent;

    private Vector3 topMessagePosBI;
    private Vector3 og_mainMessagePosBI;
    private Vector3 og_oneTextPosBI;
    public RectTransform topMessageBI;
    public RectTransform mainMessageBI;
    public RectTransform oneTextBI;

    public Coroutine beatmapInfoUI;

    public void AnimateBeatmapInfoUI()
    {
        //Reset all elements to original position
        topMessageBI.transform.position = topMessagePosBI;
        mainMessageBI.transform.position = og_mainMessagePosBI;
        oneTextBI.transform.position = og_oneTextPosBI;

        beatmapInfoUI = StartCoroutine(IE_AnimateBeatmapInfoUI());
    }

    IEnumerator IE_AnimateBeatmapInfoUI()
    {
        eventSystem.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        //Tween the top message.
        yield return LeanTween.moveX(topMessageBI, -110f, 1f).setEaseOutCirc();
        yield return new WaitForSeconds(1f);
        yield return LeanTween.moveY(oneTextBI, 50f, 0.25f).setEaseOutCirc();
        yield return new WaitForSeconds(0.15f);
        yield return LeanTween.moveY(mainMessageBI, 20f, 0.5f).setEaseOutCirc();
        yield return new WaitForSeconds(0.75f);

        //Move every input field individually, so that they come in slightly delayed to the one before it.
        for (int i = 0; i < inputFieldsParent.childCount; i++)
        {
            RectTransform inputField = inputFieldsParent.GetChild(i).GetComponent<RectTransform>();
            yield return LeanTween.moveX(inputField, 0f, 0.66f).setEaseInOutCirc();
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        eventSystem.SetActive(true);
    }
    #endregion BeatmapInfo
}
