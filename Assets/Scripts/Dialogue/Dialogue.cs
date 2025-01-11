using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Dialogue : DialogueManager
{
    [TextArea(2, 5)]
    public string[] dialogue;
    public Texture[] characterPortraits;
    private int dialogueIndex = -1;


    private void Update()
    {

    }

    public void PlayDialogue(float typingSpeed)
    {
        TypeSpeed = typingSpeed;
        dialogueIndex++;
        TypeDialogue(dialogue[dialogueIndex], characterPortraits[dialogueIndex]);
    }

    public void IncreaseSpeed(float speedIncrease) { TypeSpeed += speedIncrease; }
}
