using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private Behaviour playerHandle;
    [SerializeField] private GameObject cutsceneUI;
    [SerializeField] private PlayableDirector cutsceneClip;
    [SerializeField] private Animator topBar;
    [SerializeField] private Animator bottomBar;
    private bool CutscenePlayed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CutscenePlayed == false)
        {
            CutscenePlayed = true;
            cutsceneClip.Play();
            cutsceneUI.SetActive(true);
            topBar.SetBool("isEnding", false);
            bottomBar.SetBool("isEnding", false);
            playerHandle.enabled = false;
        }
    }

    public void CutsceneEnd()
    {
        cutsceneClip.Stop();
        playerHandle.enabled = true;
        cutsceneUI.SetActive(false);
    }

    public void RemoveBlackBars()
    {
        topBar.SetBool("isEnding", true);
        bottomBar.SetBool("isEnding", true);
    }
}
