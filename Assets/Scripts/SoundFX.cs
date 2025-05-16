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

    public void PlayRandomSFX(AudioClip[] sfx, bool RandomPitch)
    {
        int totalClips = TotalClips(sfx);

        int clipUsed = Random.Range(0, totalClips);

        PlaySFX(sfx[clipUsed], RandomPitch);
    }

    private int TotalClips(AudioClip[] sfx)
    {
        int totalClips = 0;
        foreach (AudioClip clip in sfx)
        {
            totalClips++;
        }

        return totalClips;
    }
}
