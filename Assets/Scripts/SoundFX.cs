using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundFX : MonoBehaviour
{
    [SerializeField] private AudioSource soundfxManager;
    private float randomizedpitch;

    public void PlaySFX(AudioClip sfx, bool RandomPitch)
    {
        if (RandomPitch == true)
        {
            randomizedpitch = Random.Range(0.9f, 1.1f);
            soundfxManager.pitch = randomizedpitch;
            if (sfx != null)
            {
                soundfxManager.PlayOneShot(sfx);
            }
        }
        else if (RandomPitch == false)
        {
            soundfxManager.pitch = 1;
            soundfxManager.PlayOneShot(sfx);
        }
    }
}
