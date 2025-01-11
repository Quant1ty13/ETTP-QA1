using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private RawImage characterPortrait;
    [SerializeField] protected float TypeSpeed = 10;

    private const string HTML_BLACK = "<color=#00000000>";
    private const float MaxTypeTime = 0.1f;
    protected void TypeDialogue(string dialogue, Texture portrait)
    {
        if (portrait != null)
        {
            characterPortrait.texture = portrait;
        }

        StartCoroutine(TextType(dialogue));
    }

    private IEnumerator TextType(string text)
    {
        dialogueText.text = "";

        string originalText = text;
        string displayedText = "";
        int AlphaIndex = 0;
        foreach (char c in text.ToCharArray())
        {
            AlphaIndex++;
            dialogueText.text = originalText;

            displayedText = dialogueText.text.Insert(AlphaIndex, HTML_BLACK);
            dialogueText.text = displayedText;

            yield return new WaitForSeconds(MaxTypeTime / TypeSpeed);
        }
    }
}
