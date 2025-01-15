using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxDialogue : Dialogue
{
    private bool AlreadyPlayed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StopAllCoroutines();
        dialogueBox.SetActive(true);

        if (AlreadyPlayed == false)
        {
            PlayDialogue(15);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (AlreadyPlayed == false)
        {
            AlreadyPlayed = true;            
            StartCoroutine(StopDialogue(5));
        }
    }
    private IEnumerator StopDialogue(float time)
    {
        yield return new WaitForSeconds(time);
        dialogueBox.SetActive(false);
    }
}
