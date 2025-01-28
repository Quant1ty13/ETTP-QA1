using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialCookie : BaseNPC, ICutscenable
{
    [SerializeField] private PlayableDirector cutsceneClip;
    public float CutsceneLength;
    public bool CutscenePlayed { get; private set; }

    public override void Interact()
    {
        Debug.Log("you are in range of tutorial cookie and he'll engage in dialogue conversation");
        if (CutscenePlayed == false) { PlayCutscene(); CutscenePlayed = true; }
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
}
