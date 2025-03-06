using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class CutsceneTrigger : CutsceneManager
{
    [SerializeField] private Behaviour playerHandle;
    [SerializeField] private GameObject cutsceneUI;
    [SerializeField] private PlayableDirector cutsceneClip;
    [SerializeField] private Animator topBar;
    [SerializeField] private Animator bottomBar;
    [SerializeField] private GameObject levelPassedUI;
    [SerializeField] private GameObject FirstSelect;
    private bool CutscenePlayed;
    public float cutsceneLength;
    public CinemachineVirtualCamera endingCam;
    public Dialogue dialogue;
    private int dialogueLength = -2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CutscenePlayed == false)
        {
            CutscenePlayed = true;
            cutsceneClip.Play();
            if (cutsceneUI != null)
            {
                cutsceneUI.SetActive(true);
                topBar.SetBool("isEnding", false);
                bottomBar.SetBool("isEnding", false);
            }
            playerHandle.enabled = false;
        }
    }

    public override void CutsceneSkip()
    {
        foreach (string dialogueIndex in dialogue.dialogue)
        {
            dialogueLength++;
        }
        dialogue.dialogueIndex = dialogueLength;
        cutsceneClip.time = cutsceneLength;
    }

    public void CutsceneEnd()
    {
        cutsceneClip.Stop();
        playerHandle.enabled = true;
        if (cutsceneUI != null)
        {
            cutsceneUI.SetActive(false);
        }
    }

    public void RemoveBlackBars()
    {
        topBar.SetBool("isEnding", true);
        bottomBar.SetBool("isEnding", true);
    }

    public void LevelEnd()
    {
        endingCam.Priority = 12;
        levelPassedUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstSelect);
        cutsceneClip.Stop();
        if (cutsceneUI != null)
        {
            cutsceneUI.SetActive(false);
        }
    }
}
