using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    int PlayerSpeed = 5;
    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.right * Time.deltaTime * PlayerSpeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-transform.right * Time.deltaTime * PlayerSpeed);
        }
    }
}
