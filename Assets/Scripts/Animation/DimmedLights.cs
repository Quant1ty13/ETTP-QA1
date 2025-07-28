using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DimmedLights : MonoBehaviour
{
    [SerializeField] private float HigherLimit;
    [SerializeField] private float LowerLimit;
    public float scaleModifier;
    private Light2D Light;
    private float originalIntensity;

    private bool dimIntensity;
    [SerializeField] private bool disappearWhenPlayerClose;
    [SerializeField] private float DimmedLight;
    private bool startDisappear;
    private void Awake()
    {
        Light = GetComponent<Light2D>();

        originalIntensity = Light.intensity;
    }

    void Update()
    {
        if (Light.intensity < DimmedLight)
        {
            originalIntensity = DimmedLight;
            Light.intensity = DimmedLight;
            startDisappear = false;
        }

        if (startDisappear == true)
        {
            HigherLimit = 0.1f;
            LowerLimit = 0.1f;

            Light.intensity -= (scaleModifier * 2) * Time.deltaTime;
            return;
        }

        if (Light.intensity < (originalIntensity - LowerLimit))
        {
            dimIntensity = false;
        }

        if (Light.intensity > (originalIntensity + HigherLimit))
        {
            dimIntensity = true;
        }

        if (dimIntensity)
        {
            Light.intensity -= scaleModifier * Time.deltaTime;
        }

        if (!dimIntensity)
        {
            Light.intensity += scaleModifier * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (disappearWhenPlayerClose)
        {
            startDisappear = true;
        }
    }
}
