using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Jumping")]
    public float JumpHeight;
    private bool isJumping;

    [Header("Player Movement")]
    public int WalkSpeed = 5;
    public int PlayerSprint = 8;
    public float AccelerationRate;
    public float DecelerationRate;
    private float CurrentSpeed;
    private float MaxPlayerSpeed;
    public PlayerController playerInputs;

    [Header("Miscellaneous")]
    Rigidbody2D rb2d;
    private string defineCooldown;
    public Transform groundCheck;
    public LayerMask defineGround;
    private float originalGravityScale;
    private bool Grounded() { return Physics2D.OverlapCircle(groundCheck.position, 0.35f, defineGround); }
    private Vector2 movement;

    private void Awake()
    {
        playerInputs = new PlayerController();

        playerInputs.Action.Jump.performed += jumping => Jumping(); // for future me if you want to implement Apex Hang Time, just change player movement speed for a brief period of time.
        playerInputs.Action.Jump.canceled += jumpcancel => jumpCancel();

        playerInputs.Action.Sprint.performed += sprinting => Sprinting();
        playerInputs.Action.Sprint.canceled += sprintcancel => SprintCancel();
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        MaxPlayerSpeed = WalkSpeed;
        originalGravityScale = rb2d.gravityScale;
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void Update()
    {
        movement = playerInputs.Action.Movement.ReadValue<Vector2>(); // future me, fix the issue on input system. On keyboard, if you press W or S movement will stop entirely. Must investigate the Input System.
        if (movement.x > 0) { movement.x = Mathf.Ceil(movement.x); } // for future me, the plan for omnidirectional dashing: store the value of "movement.x" in a seperate variable before rounding up.
        else { movement.x = Mathf.FloorToInt(movement.x); }
    }

    private void FixedUpdate()
    {
        // Check if player has collided with Ground Layer
        if (Grounded() == true) { rb2d.gravityScale = originalGravityScale; isJumping = false; }
        else if (Grounded() == false && isJumping == true && rb2d.velocity.y <= 0) { rb2d.gravityScale = originalGravityScale; }

        Movement();
    }

    private void Movement()
    {
                if (movement.x != 0)
                {
                    CurrentSpeed += AccelerationRate * Time.fixedDeltaTime;
                    CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, MaxPlayerSpeed);
                    rb2d.velocity = new Vector2(movement.x * CurrentSpeed, rb2d.velocity.y);
                }
                else if (movement.x == 0)
                {
                    Debug.Log("Deceleration Speed: " + CurrentSpeed);
                    CurrentSpeed -= DecelerationRate * Time.fixedDeltaTime;
                    CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, MaxPlayerSpeed);
                    rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
                    if(CurrentSpeed <= 0) { rb2d.velocity = new Vector2(0, rb2d.velocity.y); };
                }
    }

    private void Jumping()
    {
        if (Grounded() == true)
        {
            rb2d.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
            isJumping = true;
            rb2d.gravityScale = 2.4f;
        }
    }

    private void jumpCancel()
    {
        if (rb2d.velocity.y > 0f)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y * 0.5f);
        }
    }

    private void Sprinting() { MaxPlayerSpeed = PlayerSprint; }
    private void SprintCancel() { MaxPlayerSpeed = WalkSpeed; }


    /*    private IEnumerator Cooldown(float time)
        {
            Debug.Log("if this runs cooldown IEnumerator is played");
            switch (defineCooldown)
            {
                case "apex":
                    Debug.Log("cooldown is set to Apex and will soon begin cooldown");
                    yield return new WaitForSeconds(time);
                    Debug.Log("apex time applied");
                    ApexApplied = true;
                    rb2d.velocity = new Vector2(0, 0);
                    break;
            }
            defineCooldown = "";
        }*/

}


