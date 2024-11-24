using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    int PlayerSpeed = 5;
    public PlayerController playerInputs;
    bool CanDoubleJump = false;
    bool hasJumped = false;

    Rigidbody2D rb2d;
    public SpriteRenderer spriteRender;
    [SerializeField] private TrailRenderer TR;
    private Vector2 movement;

    private void Awake()
    {
        playerInputs = new PlayerController();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        TR.emitting = false;
    }
    private void Update()
    {
        movement = playerInputs.Action.Movement.ReadValue<Vector2>();
        transform.Translate(new Vector2(movement.x, movement.y) * Time.deltaTime * PlayerSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (hasJumped)
            {
                case true:
                    if (CanDoubleJump == true)
                    {
                        rb2d.AddForce(Vector2.up * 4.5f, ForceMode2D.Impulse);
                        TR.emitting = true;
                        CanDoubleJump = false;
                    }
                    break;
                case false:
                    rb2d.AddForce(Vector2.up * 8, ForceMode2D.Impulse);
                    CanDoubleJump = true;
                    hasJumped = true;
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasJumped = false;
        TR.emitting = false;

        if (collision.gameObject.CompareTag("Trap"))
        {
            spriteRender.color = Color.red;
            StartCoroutine(HurtChange());
        }

    }

    IEnumerator HurtChange()
    {
        yield return new WaitForSeconds(0.5f);
        spriteRender.color = Color.yellow;
    }
}
