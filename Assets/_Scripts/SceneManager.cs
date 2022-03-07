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

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);

        rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        image = transform.GetChild(0).GetComponent<Image>();
    }

    void Start()
    {
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
        Debug.Log("Changed scene to " + sceneToLoad + ".");

        GameObject eventSystem = GameObject.Find("EventSystem");

        //No need to set the eventSystem back to active as it gets replaced with a new one.
        //This is to stop input after scene transition has started.
        if (eventSystem != null)
            eventSystem.SetActive(false);

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

    public static void SwitchScene(string scene)
    {
        SceneManager.instance.ChangeScene(scene);
    }
}
