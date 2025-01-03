using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private PlayerHandler player;
    [SerializeField] private Animator spring_anim;
    [SerializeField] private AudioClip spring_sfx;
    private bool PlayOnce;

    void Update()
    {
        if (player.onSpring() == true)
        {
            //spring_anim.SetBool("springOn", true);
            if (PlayOnce == false)
            {
                PlayOnce = true;
                player.soundfxManager.PlaySFX(spring_sfx, false);
            }
        }
        else { /* spring_anim.SetBool("springOn", false); */PlayOnce = false; }
    }
}
