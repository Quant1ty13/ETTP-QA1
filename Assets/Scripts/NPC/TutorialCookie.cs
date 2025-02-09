using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialCookie : BaseNPC, ICutscenable, IDialogueable
{
    [SerializeField] private PlayableDirector cutsceneClip;
    public float CutsceneLength;
    public bool CutscenePlayed { get; private set; }
    [Header("Dialogue")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Dialogue scriptDialogue;
    [SerializeField] private int typeSpeed;
    [SerializeField] private CinemachineVirtualCamera dialogueCamera;

    public override void Interact()
    {
        Debug.Log("you are in range of tutorial cookie and he'll engage in dialogue conversation");
        if (CutscenePlayed == false) { PlayCutscene(); CutscenePlayed = true; }
        else { StartDialogue(scriptDialogue.dialogue, scriptDialogue.characterPortraits); }
    }

    public void PlayCutscene()
    {
        interactionSprite.enabled = false;
        playerHandler.enabled = false;
        cutsceneClip.Play();
    }
    public void EndCutscene()
    {
        interactionSprite.enabled = true;
        cutsceneClip.Stop();
        playerHandler.enabled = true;
    }

    public void SkipCutscene()
    {
        cutsceneClip.time = CutsceneLength;
    }

    public void StartDialogue(string[] dialogues, Texture[] portraits)
    {
        playerHandler.enabled = false;
        dialogueCamera.Priority = 11;
        dialogueBox.SetActive(true);
        scriptDialogue.PlayDialogue(typeSpeed);
        //Access the dialogue script and play it.
    }
}
