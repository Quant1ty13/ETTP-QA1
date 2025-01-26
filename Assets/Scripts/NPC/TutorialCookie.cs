using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialCookie : BaseNPC, ICutscenable
{
    [SerializeField] private PlayableDirector cutsceneClip;


    public override void Interact()
    {
        Debug.Log("you are in range of tutorial cookie and he'll engage in dialogue conversation");
        PlayCutscene();
    }

    public void PlayCutscene()
    {
        playerHandler.enabled = false;
        cutsceneClip.Play();
    }
    public void EndCutscene()
    {
        cutsceneClip.Stop();
        playerHandler.enabled = true;
    }
}
