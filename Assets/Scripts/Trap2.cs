using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap2 : MonoBehaviour
{

    [SerializeField] private float movementDistance = 2f;  // How far the platform moves
    [SerializeField] private float oscillationSpeed = 10f;   // Speed of the oscillation
    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the platform's new position using a sine wave
        Vector3 offset = transform.up * Mathf.Sin(Time.time * oscillationSpeed) * movementDistance;
        transform.position = startPosition + offset;
    }
}
