using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    bool Dashing = false;
    bool canDash = true;
    string whatCooldown;
    string dir;
    public float DashPower;

    int PlayerSpeed = 5;
    public PlayerController playerInputs;
    bool CanDoubleJump = false;
    public bool hasJumped = false;
    [SerializeField] GroundCheck groundCheck;

    Rigidbody2D rb2d;
    public SpriteRenderer spriteRender;
    [SerializeField] public TrailRenderer TR;
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
        if (Dashing == false)
        {
            transform.Translate(new Vector2(movement.x, movement.y) * Time.deltaTime * PlayerSpeed);
        }

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

        if (canDash == true && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.D))
            {
                dir = "right";
            }
            else if (Input.GetKey(KeyCode.A))
            {
                dir = "left";
            }

            Dash();
        }
    }

    private void Dash()
    {
        Dashing = true;
        canDash = false;
        whatCooldown = "dash";
        rb2d.gravityScale = 0;
        switch (dir)
        {
            case "left":
                rb2d.velocity = new Vector2(-DashPower, 0);
                break;
            case "right":
                rb2d.velocity = new Vector2(DashPower, 0);
                break;
        }
        dir = "";
        StartCoroutine(Cooldown());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Dashing = false;
        rb2d.gravityScale = 1.8f;

        if (collision.gameObject.CompareTag("Trap"))
        {
            groundCheck.spriteRender.color = Color.red;
            spriteRender.color = Color.red;
            whatCooldown = "hurtchange";
            StartCoroutine(Cooldown());
        }

    }

    IEnumerator Cooldown() // in hindsight this could've been an interface if i wasn't an idiot !!
    {
        switch (whatCooldown)
        {
            case "hurtchange":
                yield return new WaitForSeconds(0.5f);
                groundCheck.spriteRender.color = Color.yellow;
                spriteRender.color = Color.yellow;
                break;
            case "dash":
                yield return new WaitForSeconds(0.75f);
                Dashing = false;
                rb2d.gravityScale = 1.8f;
                rb2d.velocity = new Vector2(0, 0);
                yield return new WaitForSeconds(1.5f);
                canDash = true;
                break;
        }
        whatCooldown = "";

    }
}
