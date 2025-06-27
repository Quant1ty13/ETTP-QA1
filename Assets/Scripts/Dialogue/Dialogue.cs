using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Dialogue : DialogueManager
{
    [TextArea(2, 5)]
    public string[] dialogue;
    public Texture[] characterPortraits;
    [HideInInspector] public int dialogueIndex = -1;
    [SerializeField] protected GameObject dialogueBox;
    public void PlayDialogue(float typingSpeed)
    {
        TypeSpeed = typingSpeed;
        dialogueIndex++;


        if (!isTyping && dialogueIndex != dialogue.Length)
        {
            Debug.Log("checking if this is constantly running");
            TypeDialogue(dialogue[dialogueIndex], characterPortraits[dialogueIndex]);
        }
        else if (dialogueIndex >= dialogue.Length) {return; }
    }

    public void IncreaseSpeed(float speedIncrease) { TypeSpeed += speedIncrease; }
    public void DecreaseSpeed(float speedDecrease) { TypeSpeed -= speedDecrease; }

    protected void StopDialogue()
    {
        Debug.Log("stopping dialogue");
        StopCoroutine(dialogue[dialogueIndex]);
        dialogueIndex = -1;
        isTyping = false;
        dialogueBox.SetActive(false);
    }
}
