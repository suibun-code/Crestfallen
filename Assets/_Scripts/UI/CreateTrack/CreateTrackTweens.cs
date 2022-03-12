using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTrackTweens : Singleton<CreateTrackTweens>
{
    private void Start()
    {
        titleTextUM = titleUM.GetChild(0).GetComponent<TextMeshProUGUI>();
        newTitleColorUM = new Color(titleTextUM.color.r, titleTextUM.color.g, titleTextUM.color.b, 0f);

        StartCoroutine(IE_AnimateUploadMusicUI());
    }

    #region SelectMusic
    [Space]
    [Header("Upload Music")]

    public GameObject eventSystem;

    private TextMeshProUGUI titleTextUM;
    private Color newTitleColorUM;

    public RectTransform titleUM;
    public RectTransform topMessageUM;
    public RectTransform mainMessageUM;
    public RectTransform oneTextUM;

    public RectTransform refreshRect;
    private bool doneSpinning = true;

    public Coroutine IE_animateAudioFileCells;

    private IEnumerator IE_AnimateUploadMusicUI()
    {
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
    public RectTransform topMessageBI;
    public RectTransform mainMessageBI;
    public RectTransform oneTextBI;

    public void AnimateBeatmapInfoUI()
    {
        StartCoroutine(IE_AnimateBeatmapInfoUI());
    }

    public void DisableEventSystem(float seconds)
    {
        StartCoroutine(IE_DisableEventSystem(seconds));
    }

    IEnumerator IE_DisableEventSystem(float seconds)
    {
        eventSystem.SetActive(false);

        yield return new WaitForSeconds(seconds);

        eventSystem.SetActive(true);
    }

    IEnumerator IE_AnimateBeatmapInfoUI()
    {
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
    }
    #endregion BeatmapInfo
}
