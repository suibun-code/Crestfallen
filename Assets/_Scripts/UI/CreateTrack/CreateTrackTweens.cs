using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateTrackTweens : MonoBehaviour
{
    private TextMeshProUGUI title;
    public RectTransform titleRect;
    private Color newTitleColor;

    public RectTransform topMsgRect;

    public RectTransform refreshRect;
    private bool doneSpinning = true;

    private void Start()
    {
        title = titleRect.GetChild(0).GetComponent<TextMeshProUGUI>();
        newTitleColor = new Color(title.color.r, title.color.g, title.color.b, 0f);

        StartCoroutine(IE_AnimateUploadMusicUI());
    }

    private IEnumerator IE_AnimateUploadMusicUI()
    {
        yield return LeanTween.moveX(titleRect, 0f, 1f).setEaseOutCirc();

        while (title.color.a < 1f)
        {
            newTitleColor.a += 1f * Time.deltaTime;
            title.color = newTitleColor;
            yield return null;
        }

        yield return LeanTween.moveX(topMsgRect, 110f, 1f).setEaseOutCirc();

        yield return new WaitForSeconds(1f);

        eventSystem.SetActive(true);
    }

    public void SpinRefreshIcon()
    {
        if (doneSpinning)
            LeanTween.rotateAroundLocal(refreshRect, Vector3.forward, -360f, 1f).setEaseOutCirc().setOnComplete(SetDoneSpinning);
        refreshRect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        doneSpinning = false;
    }

    private void SetDoneSpinning()
    {
        doneSpinning = true;
    }

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
