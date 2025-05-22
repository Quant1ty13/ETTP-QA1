using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


// All instances of Ambience could be moved into it's own script if there's a need be.
public class SoundFX : MonoBehaviour
{
    [SerializeField] private AudioSource soundfxManager;
    private float randomizedpitch;

    //AMBIENCE VARIABLES
    public BaseAmbience ambience { get; set; }
    private float counter;

    private void Start()
    {
        counter = Random.Range(1, 4);
        ambience.lowestAmbienceWait = ambience.maxAmbienceWait - 6;
    }

    private void Update()
    {
        if (counter <= 0)
        {
            PlayAmbience();
        }
        else if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
    }

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

    private void PlayAmbience()
    {
        if (ambience == null)
        {
            return;
        }

        float ambienceWaitTime = Random.Range(ambience.lowestAmbienceWait, ambience.maxAmbienceWait);

        Debug.Log("Playing Ambience!");
        counter = ambienceWaitTime;
        PlayRandomSFX(ambience.ambienceSFX, true);
    }
}
