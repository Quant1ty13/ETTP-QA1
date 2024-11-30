using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float originalGravity;
    bool Dashing = false;
    bool canDash = true;
    bool dashJump = false;
    string whatCooldown;
    string dir;
    public float DashPower;

    public int PlayerSpeed = 5;
    public int PlayerSprint = 8;
    private bool Sprinting = false;
    public PlayerController playerInputs;

    public float jumpDash = 5f;
    public float sprintJump = 6f;
    public float JumpHeight = 10;
    public float Coyotetime = 0.2f;
    private float timeAfterJump;
    public bool hasJumped = false;
    private bool onGround = true;

    [SerializeField] Rigidbody2D rb2d;
    public SpriteRenderer spriteRender;
    [SerializeField] public TrailRenderer TR;
    private Vector2 movement;

    private void Awake()
    {
        originalGravity = rb2d.gravityScale;
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
        if (onGround != true)
        {
            timeAfterJump -= Time.deltaTime;
        }
        else
        {
            timeAfterJump = Coyotetime;
        }

        if (timeAfterJump < 0)
        {
            hasJumped = true;
        }

        movement = playerInputs.Action.Movement.ReadValue<Vector2>();
        if (Dashing == false)
        {
            if (Sprinting == true)
            {
                transform.Translate(new Vector2(movement.x, movement.y) * Time.deltaTime * PlayerSprint);
            }
            else
            {
                transform.Translate(new Vector2(movement.x, movement.y) * Time.deltaTime * PlayerSpeed);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && hasJumped == false)
        {
            Debug.Log("Jumped");
            if (dashJump == true)
            {
                rb2d.AddForce(Vector2.up * jumpDash, ForceMode2D.Impulse);
            }
            else if (Sprinting == true)
            {
                rb2d.AddForce(Vector2.up * sprintJump, ForceMode2D.Impulse);
            }
            else
            {
                rb2d.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
            }
            hasJumped = true;
        }

        if (canDash == true && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.D))
            {
                dir = "right";
                Dash();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                dir = "left";
                Dash();
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Sprinting = true;
        }

        if (!Input.GetKey(KeyCode.LeftControl) && Sprinting == true)
        {
            Sprinting = false;
        }
    }

    private void Dash()
    {
        hasJumped = false;
        onGround = true;
        dashJump = true;
        TR.emitting = true;
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            hasJumped = false;
            TR.emitting = false;
        }

        if (Dashing == true)
        {
            Dashing = false;
            rb2d.gravityScale = originalGravity;
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            spriteRender.color = Color.red;
            whatCooldown = "hurtchange";
            StartCoroutine(Cooldown());
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    IEnumerator Cooldown() // in hindsight this could've been an interface if i wasn't an idiot !!
    {
        switch (whatCooldown)
        {
            case "hurtchange":
                yield return new WaitForSeconds(0.5f);
                spriteRender.color = Color.yellow;
                break;
            case "dash":
                yield return new WaitForSeconds(0.75f);
                onGround = false;
                Dashing = false;
                dashJump = false;
                rb2d.gravityScale = originalGravity;
                rb2d.velocity = new Vector2(0, 0);
                yield return new WaitForSeconds(1.5f);
                canDash = true;
                break;
        }
        whatCooldown = "";

    }
}
