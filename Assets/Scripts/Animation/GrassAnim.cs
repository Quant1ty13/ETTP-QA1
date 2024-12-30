using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAnim : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    public Sprite[] sprite_anim;
    private int TotalSprites;
    public int StartingSprite;
    private int CurrentSprite;
    public float MinimumTime;
    public float MaximumTime;
    private float TimeToPlayAnim;
    private float TimeCounter;

    private void Start()
    {
        CurrentSprite = StartingSprite;
        sr.sprite = sprite_anim[CurrentSprite];

        TotalSprites = sprite_anim.Length - 1;
        Debug.Log(TotalSprites);
    }

    private void Update()
    {
        if (TimeCounter == 0)
        {
            TimeCounter = Random.Range(MinimumTime, MaximumTime);
        }
        else if (TimeCounter != 0)
        {
            TimeToPlayAnim += Time.deltaTime;
        }
        else { };

        if (TimeToPlayAnim >= TimeCounter)
        {
            TimeCounter = 0;
            TimeToPlayAnim = 0;
            AnimationUpdate();
        }
    }

    private void AnimationUpdate()
    {
        if (CurrentSprite <= TotalSprites)
        {
            CurrentSprite += 1;
            Debug.Log(CurrentSprite);
        }
        else { };

        if (CurrentSprite >= TotalSprites)
        {
            CurrentSprite = 0;
        }
        else { };

        sr.sprite = sprite_anim[CurrentSprite];
    }
}
