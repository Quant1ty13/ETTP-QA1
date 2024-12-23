using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundFX : MonoBehaviour, IPointerEnterHandler
{
    public SoundFX soundfxManager;
    public AudioClip hover_button;
    public AudioClip button_click;
    public void OnPointerEnter(PointerEventData eventData)
    {
        soundfxManager.PlaySFX(hover_button, true);
    }

    public void ClickEffect()
    {
        soundfxManager.PlaySFX(button_click, false);
    }
}
