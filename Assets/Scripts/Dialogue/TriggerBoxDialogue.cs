using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxDialogue : Dialogue
{
    private bool AlreadyPlayed;
    public bool allowWallClimbTrigger; // please please for the love of god if your gonna add one more extra trigger box PLEASE make OnTriggerStay dependent on a SO. thank you
    public PlayerHandler playerHandler;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (allowWallClimbTrigger == false)
        {
            StopAllCoroutines();

            if (AlreadyPlayed == false)
            {
                dialogueBox.SetActive(true);
                PlayDialogue(TypeSpeed);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isTyping && EndDialogue == true)
        {
            StopDialogue();
        }

        if (allowWallClimbTrigger == true && playerHandler.EnableWallClimbing == true)
        {
            PlayDialogue(TypeSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopDialogue();
    }
}
