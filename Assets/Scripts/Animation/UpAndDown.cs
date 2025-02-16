using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    [SerializeField] private int animationSpeed;
    [SerializeField] private float height;

    private void Update()
    {
        Vector3 object_pos = this.transform.position;
        // Use Lerp, Clamp and Cosine to Move up and Down
        float newY = Mathf.Sin(Time.time * animationSpeed);

        transform.position = new Vector3(object_pos.x, object_pos.y + newY * height, object_pos.z);
    }
}
