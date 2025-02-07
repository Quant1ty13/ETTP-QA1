using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxDialogue : Dialogue
{
    private bool AlreadyPlayed;
    public bool allowWallClimbTrigger; // please please for the love of god if your gonna add one more extra trigger box PLEASE make OnTriggerStay dependent on a SO. thank you
    public PlayerHandler playerHandler;
    public float timeUntilDialogueDissapears;
    [SerializeField] private bool PersistentDialogue;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (allowWallClimbTrigger == false)
        {
            StopAllCoroutines();

            if (AlreadyPlayed == false)
            {
                dialogueBox.SetActive(true);
                PlayDialogue(15);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (allowWallClimbTrigger == true && playerHandler.EnableWallClimbing == true)
        {
            PlayDialogue(15);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (AlreadyPlayed == false)
        {
            AlreadyPlayed = true;
            if (PersistentDialogue == true)
            {
                return;
            }
            StartCoroutine(StopDialogue(timeUntilDialogueDissapears));
        }
    }
    private IEnumerator StopDialogue(float time)
    {
        yield return new WaitForSeconds(time);
        dialogueBox.SetActive(false);
    }
}
