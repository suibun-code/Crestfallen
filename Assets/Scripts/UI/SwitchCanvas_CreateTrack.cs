using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCanvas_CreateTrack : MonoBehaviour
{
    public GameObject eventSystem;

    public Camera cameraRef;
    public Color backgroundColorAfterSwitch;

    public Canvas toSwitch;
    public Canvas switchTo;

    public RectTransform rect_topMsg;
    public Transform inputFieldsParent;

    public void Switch()
    {
        eventSystem.SetActive(false);
        toSwitch.enabled = false;
        switchTo.enabled = true;
        SongManager.instance.StopMusic();
    }

    public void ChangeBackgrondColor()
    {
        cameraRef.backgroundColor = backgroundColorAfterSwitch;
    }

    public void AnimateBeatmapInfoUI()
    {
        StartCoroutine(IE_AnimateBeatmapInfoUI());
    }

    IEnumerator IE_AnimateBeatmapInfoUI()
    {
        yield return LeanTween.moveX(rect_topMsg, 110f, 1f).setEaseOutCirc();

        for (int i = 0; i < inputFieldsParent.childCount; i++)
        {
            RectTransform inputField = inputFieldsParent.GetChild(i).GetComponent<RectTransform>();
            yield return LeanTween.moveX(inputField, 0f, 0.66f).setEaseInOutCirc();
            yield return new WaitForSeconds(0.1f);
        }

        eventSystem.SetActive(true);
    }
}
