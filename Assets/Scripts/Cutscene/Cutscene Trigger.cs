using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private Behaviour playerHandle;
    [SerializeField] private GameObject cutsceneUI;
    [SerializeField] private PlayableDirector cutsceneClip;
    private bool CutscenePlayed;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CutscenePlayed == false)
        {
            CutscenePlayed = true;
            cutsceneClip.Play();
            cutsceneUI.SetActive(true);
            playerHandle.enabled = false;
        }
    }

    public void CutsceneEnd()
    {
        cutsceneClip.Stop();
        playerHandle.enabled = true;
        cutsceneUI.SetActive(false);
    }
}
