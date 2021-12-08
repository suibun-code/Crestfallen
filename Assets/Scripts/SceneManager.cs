using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneManager : Singleton<SceneManager>
{
    private RectTransform rectTransform;
    private Image image;

    public float transitionTime;

    new void Awake() 
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        image = transform.GetChild(0).GetComponent<Image>();

        StartCoroutine(StartFade());
    }

    public void ChangeScene(string sceneToLoad)
    {
        StartCoroutine(cr_ChangeScene(sceneToLoad));
    }

    IEnumerator StartFade()
    {
        yield return WaitForSceneLoad("MainMenu");
        yield return LeanTween.alpha(rectTransform, 0f, transitionTime);
        yield return new WaitForSeconds(transitionTime);
    }

    IEnumerator cr_ChangeScene(string sceneToLoad)
    {
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        if (eventSystem != null)
            eventSystem.enabled = false;

        //Exit scene
        yield return LeanTween.alpha(rectTransform, 1f, transitionTime);
        yield return new WaitForSeconds(transitionTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);

        //Enter scene
        yield return StartCoroutine(WaitForSceneLoad(sceneToLoad));
        yield return LeanTween.alpha(rectTransform, 0f, transitionTime);
        yield return new WaitForSeconds(transitionTime);
    }

    IEnumerator WaitForSceneLoad(string sceneName)
    {
        while (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(sceneName))
            yield return null;
    }
}
