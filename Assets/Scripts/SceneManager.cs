using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : Singleton<SceneManager>
{
    private RectTransform rectTransform;
    private Image image;

    public float transitionTime;

    new void Awake()
    {
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
        yield return LeanTween.alpha(rectTransform, 1f, transitionTime);
        yield return new WaitForSeconds(transitionTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        
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
