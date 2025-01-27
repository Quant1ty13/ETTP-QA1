using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialCookie : BaseNPC, ICutscenable
{
    [SerializeField] private PlayableDirector cutsceneClip;
    public float CutsceneLength;

    public override void Interact()
    {
        Debug.Log("you are in range of tutorial cookie and he'll engage in dialogue conversation");
        PlayCutscene();
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
