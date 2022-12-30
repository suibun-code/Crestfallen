using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Edit : MonoBehaviour
{
    [SerializeField] private Timeline timeline;

    private void OnEnable()
    {
        //Debug.Log("ENABLED");
        SongManager.OnNewSong += RestartTimeline;
    }

    private void OnDisable()
    {
        //Debug.Log("DISABLED");
        SongManager.OnNewSong -= RestartTimeline;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void RestartTimeline()
    {
        Debug.Log("RESTARTED TIMELINE");
        timeline.DestroyTimeline();
        timeline.InitTimeline();
    }

    public void OnEscape(InputValue value)
    {
        //Switch menu, print debug error if scene from string not found
        MenuManager.instance.SwitchMenu("///MainMenu (Scene)");
    }
}
