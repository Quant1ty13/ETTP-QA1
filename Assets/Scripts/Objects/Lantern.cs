using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lantern : MonoBehaviour
{
    private float timeCounter;
    private float waitPeriod;
    private bool enableWaitTime;
    private float maxAngle = 3f;
    private float minAngle = -3f;
    private float rate;

    void Update()
    {
        // Wait Period
        if (enableWaitTime == true)
        {
            if (waitPeriod == 0)
            {
                waitPeriod = Random.Range(0.15f, 0.35f);
            }

            timeCounter += Time.deltaTime;


            if (timeCounter > waitPeriod)
            {
                enableWaitTime = false;
                timeCounter = 0;
                waitPeriod = 0;
                return;
            }
        }

        // Rotate
        transform.Rotate(transform.rotation.x, transform.rotation.y, Mathf.Lerp(minAngle, maxAngle, rate) / 85);
        
        // Exponentially Increase the Lerped Value;
        rate += 0.5f * Time.deltaTime;

        // Reset;
        if (rate > 1.0f)
        {
            float tempMax = maxAngle;
            maxAngle = minAngle;
            minAngle = tempMax;
            rate = 0f;
            enableWaitTime = true;
            return;
        }
    }
}
