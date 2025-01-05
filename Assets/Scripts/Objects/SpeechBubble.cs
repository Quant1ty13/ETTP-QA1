using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    private Vector2 startingPosition;
    private float MinimumYThreshold;
    void Start()
    {
        startingPosition = transform.position; 
        MinimumYThreshold = this.transform.position.y - 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < MinimumYThreshold)
        {
            this.transform.position = startingPosition;
        }
    }
}
