using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using System;

public class PlayerHandler : PlayerStat
{
    [Header("Jumping")]
    public float JumpHeight;
    public float JumpBufferTime;
    public float CoyoteTime;
    public float ApexHangTime;
    public bool canJump { get; private set; }
    public bool isJumping { get; private set; }
    public float coyoteTimeCounter { get; private set; }
    public float timeSinceJump { get; private set; }
    public float timeHoldingJump { get; private set; }
    public bool enableJumpBuffer { get; private set; }
    public float apexHangCounter { get; private set; }
    public bool jumpActivate { get; private set; }

    [Header("Player Movement")]
    public int WalkSpeed;
    public int PlayerSprint;
    public float AccelerationRate;
    public float DecelerationRate;
    public float DashSpeed;
    public float DashTime;
    public float BonusHeight_Dash;
    public float BonusSpeed_Dash;
    public float BonusSpeedTime;
    public bool dashActivate { get; private set; }
    public bool isDashing { get; private set; }
    public float dashCounter { get; private set; }
    public float bonusHeightCounter { get; private set; }
    public bool hasDashed { get; private set; }
    public float bonusSpeedCounter { get; private set; }
    private float currentSpeed;
    private float maxPlayerSpeed;
    public PlayerController playerInputs;

    [Header("Particle Effects")]
    public ParticleSystem dashParticle;

    [Header("Wall Climbing")]
    public float ClimbingSpeed;
    public bool enableWallClimbing { get; private set; }
    public bool onLeftWall() { return Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, 0.25f, defineClimbableWall); }
    public bool onRightWall() { return Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, 0.25f, defineClimbableWall); }

    [Header("Audio Variables")]
    public SoundFX soundfxManager;
    public AudioClip jump;
    public AudioClip dash;
    [SerializeField] private GameObject PauseMenu;
    public AudioSource music;
    private bool PauseMenuOn;

    [Header("Miscellaneous")]
    public Rigidbody2D rb2d;
    private string defineCooldown;
    public Transform groundCheck;
    public Animator player_animation;
    public SpriteRenderer sr;
    public LayerMask defineGround;
    public LayerMask defineClimbableWall;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float originalGravityScale;
    public bool onGround() { return Physics2D.OverlapCircle(groundCheck.position, 0.25f, defineGround); }

    private Vector2 movement;

    #region State Variables
    BaseState currentState;
    StatesHandler states;
    public BaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public bool CanJump { get { return canJump; } set { canJump = value; } }
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    public float CoyoteTimeCounter { get { return coyoteTimeCounter; } set { coyoteTimeCounter = value; } }
    public float TimeSinceJump { get { return timeSinceJump; } set { timeSinceJump = value; } }
    public float TimeHoldingJump { get { return timeHoldingJump; } set { timeHoldingJump = value; } }
    public bool EnableJumpBuffer { get { return enableJumpBuffer; } set { enableJumpBuffer = value; } }
    public float ApexHangCounter { get { return apexHangCounter; } set { apexHangCounter = value; } }
    public bool JumpActivate { get { return jumpActivate; } set { jumpActivate = value; } }
    public Vector2 Movement { get { return movement; } }
    public float CurrentSpeed { get { return currentSpeed; } set { currentSpeed = value; } }
    public float MaxPlayerSpeed { get { return maxPlayerSpeed; } set { maxPlayerSpeed = value; } }
    public float BonusHeightCounter { get { return bonusHeightCounter; } set { bonusHeightCounter = value; } }

    // Dashing
    public bool DashActivate { get { return dashActivate; } set { dashActivate = value; } }
    public bool IsDashing { get { return isDashing; } set { isDashing = value; } }
    public float DashCounter { get { return dashCounter; } set { dashCounter = value; } }
    public bool HasDashed { get { return hasDashed; } set { hasDashed = value; } }
    public float BonusSpeedCounter { get { return bonusSpeedCounter; } set { bonusSpeedCounter = value; } }

    // Wall Climbing
    public bool EnableWallClimbing { get { return enableWallClimbing; } set { enableWallClimbing = value; } }
    #endregion

    private void Awake()
    {
        playerInputs = new PlayerController();

        playerInputs.Action.Pause.performed += enablepause => EnablePauseMenu();

        playerInputs.Action.Dash.performed += dash_performed => DashPerformed();

        playerInputs.Action.Jump.started += jumpactivating => activeJump();
        playerInputs.Action.Jump.performed += jumpactivate => activeJump();
        playerInputs.Action.Jump.canceled += jumpcancel => jumpCancel();

        playerInputs.Action.Sprint.performed += sprinting => Sprinting();
        playerInputs.Action.Sprint.canceled += sprintcancel => SprintCancel();

        playerInputs.Action.Climbing.performed += climbing_performed => ClimbingPerformed();
        playerInputs.Action.Climbing.canceled += exit_climb => ClimbingCanceled();
        // setup state
        states = new StatesHandler(this);
        currentState = states.Grounded();
    }
    private void Start()
    {


        music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        music.volume = PlayerPrefs.GetFloat("MusicVolume");

        rb2d = GetComponent<Rigidbody2D>();
        player_animation = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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

    // State Manager 

    private void Update()
    {
        movement = playerInputs.Action.Movement.ReadValue<Vector2>(); // future me, fix the issue on input system. On keyboard, if you press W or S movement will stop entirely. Must investigate the Input System.
        if (movement.x > 0) { movement.x = Mathf.Ceil(movement.x); } // for future me, the plan for omnidirectional dashing: store the value of "movement.x" in a seperate variable before rounding up.
        else { movement.x = Mathf.FloorToInt(movement.x); }
        currentState.UpdateStates();

    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateStates();
    }


    // Other functions needed: 
    private void jumpCancel()
    {
        // revise this if else statement to accomodate for jump buffering.
        if (rb2d.velocity.y > 0f)
        {
            timeHoldingJump = 0;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y * 0.5f);
        }
    }


    // Collision Check
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Reset"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the scene
        }
    }
    // PUT EVERYTHING BELOW HERE INTO A DIFFERENT PAUSE MENU SCRIPT
    private void EnablePauseMenu()
    {
        Debug.Log("is this work?");
        if (PauseMenuOn == true)
        {
            ExitPause();
        }
        else if (PauseMenuOn == false)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0f;
            PauseMenuOn = true;
        }
    }

    public void ExitPause()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        PauseMenuOn = false;
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void activeJump() { jumpActivate = true; }
    private void Sprinting() { MaxPlayerSpeed = PlayerSprint; }
    private void SprintCancel() { MaxPlayerSpeed = WalkSpeed; }
    private void DashPerformed() { dashActivate = true; }
    private void ClimbingPerformed()
    {
        // Detect when a climbable wall is nearby
        if (onLeftWall() == true)
        {
            Debug.Log("Climbable Wall on the left");
            enableWallClimbing = true;
        }

        if (onRightWall() == true)
        {
            Debug.Log("Climbable Wall on the right");
            enableWallClimbing = true;
        }
        

        // Probably set a boolean to true?
    }
    private void ClimbingCanceled() 
    {
        enableWallClimbing = false;
    }

    public void StartCountdown() { StopCoroutine(Cooldown()); StartCoroutine(Cooldown()); }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(BonusSpeedTime);
        bonusSpeedCounter = 0;
    }
}
