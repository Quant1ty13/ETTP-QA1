using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap3 : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f; // Degrees per second
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Axis of rotation

    void Update()
    {
        // Rotate the obstacle around the specified axis
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    // Detect collisions with the player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle player collision (e.g., damage, game over)
            Debug.Log("Player collided with rotating obstacle!");

            // Example:  Reset player position
            //collision.gameObject.transform.position = new Vector3(0, 1, 0);  // Adjust as needed
        }
    }
}
