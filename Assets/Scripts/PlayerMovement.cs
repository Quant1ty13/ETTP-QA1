using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    int PlayerSpeed = 5;
    bool Jumped = false;
    bool DoubleJump = false;

    Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Jumped == false)
            {
                rb2d.AddForce(Vector2.up * 9, ForceMode2D.Impulse);
                Jumped = true;
            }
            else if (DoubleJump == false)
            {
                rb2d.AddForce(Vector2.up * 4.5f, ForceMode2D.Impulse);
                DoubleJump = true;
            }
            else
            {
                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Jumped = false;
        DoubleJump = false;
    }
}
