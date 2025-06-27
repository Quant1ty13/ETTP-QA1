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
    [SerializeField] protected SoundFX soundfxManager;
    [SerializeField] protected AudioClip typingAudio;
    public AudioSource sfx;
    private bool AudioPlaying;
    [SerializeField] protected bool EndDialogue = false;
    private const string HTML_BLACK = "<color=#00000000>";
    private const float MaxTypeTime = 0.1f;
    [SerializeField] protected bool isTyping;

    private void Start()
    {
        sfx.volume = PlayerPrefs.GetFloat("SoundFXVolume");
    }

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
        isTyping = true;
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

            if (AudioPlaying == false)
            {
                StartCoroutine(TextAudio(typingAudio));
            }
            else { };

            yield return new WaitForSeconds(MaxTypeTime / TypeSpeed);
        }

        isTyping = false;
    }

    private IEnumerator TextAudio(AudioClip typeaudio)
    {
        AudioPlaying = true;
        yield return new WaitForSeconds(0.1f);
        soundfxManager.PlaySFX(typeaudio, true);
        AudioPlaying = false;
    }
}
