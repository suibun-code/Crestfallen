using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTrackTweens : Singleton<CreateTrackTweens>
{
    #region SelectMusic

    public Coroutine IE_animateAudioFileCells;

    private TextMeshProUGUI title;
    public RectTransform titleRect;
    private Color newTitleColor;

    public RectTransform topMsgRect;
    public RectTransform mainMsgRect;
    public RectTransform oneTextRect;

    public RectTransform refreshRect;
    private bool doneSpinning = true;

    private void Start()
    {
        title = titleRect.GetChild(0).GetComponent<TextMeshProUGUI>();
        newTitleColor = new Color(title.color.r, title.color.g, title.color.b, 0f);

        StartCoroutine(IE_AnimateCreateTrackUI());
    }

    private IEnumerator IE_AnimateCreateTrackUI()
    {
        yield return new WaitForSeconds(0.5f);

        yield return LeanTween.moveX(titleRect, 0f, 1f).setEaseOutCirc();

        while (title.color.a < 1f)
        {
            newTitleColor.a += 0.75f * Time.deltaTime;
            title.color = newTitleColor;
            yield return null;
        }

        yield return LeanTween.moveX(topMsgRect, -110f, 1f).setEaseOutCirc();
        yield return new WaitForSeconds(1f);
        yield return LeanTween.moveY(oneTextRect, 50f, 0.25f).setEaseOutCirc();
        yield return new WaitForSeconds(0.15f);
        yield return LeanTween.moveY(mainMsgRect, 20f, 0.5f).setEaseOutCirc();
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

    public GameObject eventSystem;
    public RectTransform rectTopMsg;
    public Transform inputFieldsParent;

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
        //Tween the top message.
        yield return LeanTween.moveX(rectTopMsg, 110f, 1f).setEaseOutCirc();

        //Move every input field individually, so that they come in slightly delayed to the one before it.
        for (int i = 0; i < inputFieldsParent.childCount; i++)
        {
            RectTransform inputField = inputFieldsParent.GetChild(i).GetComponent<RectTransform>();
            yield return LeanTween.moveX(inputField, 0f, 0.66f).setEaseInOutCirc();
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion BeatmapInfo
}
