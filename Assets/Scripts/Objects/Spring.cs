using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private Sprite[] sprite_anim;
    private SpriteRenderer sr;
    private int totalSprites;
    private int currentSprite = 0;
    [SerializeReference] private float timeForEachSprite;
    private float timeCounter;
    private float stopCounter;
    private bool activateAnim;
    [SerializeField] private PlayerHandler player;
    [SerializeField] private Transform TopCheck;
    [SerializeField] private LayerMask Player;
    private bool temporaryStop;
    public bool topCheck() { return Physics2D.OverlapCircle(TopCheck.position, 0.55f, Player); }

    private bool restartCycle;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        timeCounter = timeForEachSprite;
        totalSprites = sprite_anim.Length - 1;
    }

    void Update()
    {
        if ((topCheck() && player.onSpring() == true) && activateAnim == false )
        {
            activateAnim = true;
        }

        if (activateAnim == true && temporaryStop == false)
        {
            timeCounter -= Time.deltaTime;
        }
        else if(temporaryStop == true)
        {
            timeCounter = timeForEachSprite;
            stopCounter -= Time.deltaTime;
        }

        if (stopCounter <= 0)
        {
            temporaryStop = false;
        }

        if (timeCounter < 0)
        {
            timeCounter = timeForEachSprite;
            AnimationUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        activateAnim = true;
    }

    private void AnimationUpdate()
    {
        if (currentSprite == 0 && restartCycle == true)
        {
            Debug.Log("restarting cycle!");
            activateAnim = false;
            restartCycle = false;
            return;
        }

        if (currentSprite >= totalSprites && temporaryStop == false && restartCycle == false)
        {
            Debug.Log("sprite count is greater than/equal to the total sprites, starting to restart cycle now");
            stopCounter = 0.15f;
            temporaryStop = true;
            restartCycle = true;
        }

        if (temporaryStop == true)
        {
            return;
        }

        if (currentSprite < totalSprites && restartCycle == false)
        {
            Debug.Log("increasing the sprite by 1");
            currentSprite += 1;
        }
        else if (restartCycle == true && currentSprite != 0)
        {
            Debug.Log("descreasing sprite by 1");
            currentSprite -= 1;
        }

        sr.sprite = sprite_anim[currentSprite];
    }
}
