using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{
    private float startPos, length;
    public GameObject MainCam;
    public float parralaxEffect;
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (MainCam.transform.position.x * (1 - parralaxEffect));
        float dist = (MainCam.transform.position.x * parralaxEffect);

        transform.position = new Vector2(startPos + dist, transform.position.y);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - 1) startPos -= length;
    }
}
