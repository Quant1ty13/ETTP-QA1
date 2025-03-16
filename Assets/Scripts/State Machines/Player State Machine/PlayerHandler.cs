using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
public class PlayerHandler : MonoBehaviour
{
    protected PlayerInput playerInput;

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
    public float DashShakeForce;
    public float DashSpeed;
    public float DashTime;
    public float BonusHeight_Dash;
    public float BonusSpeed_Dash;
    public float BonusSpeedTime;
    public float MaxFallSpeed;
    public float DashJumpGracePeriod;
    public bool dashActivate { get; private set; }
    public bool isDashing { get; private set; }
    public float dashCounter { get; private set; }
    public float bonusHeightCounter { get; private set; }
    public bool hasDashed { get; private set; }
    public float bonusSpeedCounter { get; private set; }
    public bool enableDashJumpGP { get; private set; }
    private float currentSpeed;
    private float maxPlayerSpeed;

    [Header("Particle Effects & Overlays")]
    public ParticleSystem dashParticle;
    public GameObject dashOverlay;
    public Animator dash_animation;

    [Header("Wall Climbing")]
    public float ClimbingSpeed;
    public float ClimbingCooldown;
    public float AutomaticClimbingCooldown;
    public float climbingCounter { get; private set; }
    public bool enableWC_Cooldown { get; private set; }
    public bool enableWallClimbing { get; private set; }
    public bool onLeftWall() { return Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, 0.25f, defineClimbableWall); }
    public bool onRightWall() { return Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, 0.25f, defineClimbableWall); }

    [Header("Audio Variables")]
    public SoundFX soundfxManager;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip spikeHurt;
    public AudioClip lavaHurt;
    public PauseMenu pausemenu_script;
    public AudioSource music;
    public AudioSource sfx;

    [Header("Miscellaneous")]
    public float SpringPower;
    public Rigidbody2D rb2d;
    public Transform groundCheck;
    public Animator player_animation;
    public SpriteRenderer sr;
    public LayerMask defineGround;
    public LayerMask defineSprings;
    public LayerMask defineClimbableWall;
    public ButtonPlay buttons;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float originalGravityScale;
    private string isAutomaticWallClimbingOn;
    private const float INTERACTION_TIMER = 0.1f;
    private float interaction_timer_counter;
    public List<int> KeyList = new List<int>();
    public bool onGround() { return Physics2D.OverlapCircle(groundCheck.position, 0.25f, defineGround); }
    public bool onSpring() { return Physics2D.OverlapCircle(groundCheck.position, 0.25f, defineSprings); }
    public Vector2 lastCheckpointLocation;
    public bool checkInteraction { get; private set; }
    public bool CheckInteraction { get { return checkInteraction; } set { CheckInteraction = value; } }
    public bool gameConcluded { get; private set; }
    public bool GameConcluded { get { return gameConcluded; } set { gameConcluded = value; } }
    public CinemachineImpulseSource impulseSource { get; private set; }

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
    public Vector2 Movement { get { return playerInput.movement; } set { playerInput.movement = value; } } // remove set {movement = value;} later on to see if it'll cause issues.
    public float CurrentSpeed { get { return currentSpeed; } set { currentSpeed = value; } }
    public float MaxPlayerSpeed { get { return maxPlayerSpeed; } set { maxPlayerSpeed = value; } }
    public float BonusHeightCounter { get { return bonusHeightCounter; } set { bonusHeightCounter = value; } }

    // Dashing
    public bool DashActivate { get { return dashActivate; } set { dashActivate = value; } }
    public bool IsDashing { get { return isDashing; } set { isDashing = value; } }
    public float DashCounter { get { return dashCounter; } set { dashCounter = value; } }
    public bool HasDashed { get { return hasDashed; } set { hasDashed = value; } }
    public float BonusSpeedCounter { get { return bonusSpeedCounter; } set { bonusSpeedCounter = value; } }
    public bool EnableDashJumpGP { get { return enableDashJumpGP; } set { enableDashJumpGP = value; } }

    // Wall Climbing
    public bool EnableWallClimbing { get { return enableWallClimbing; } set { enableWallClimbing = value; } }
    public bool EnableWC_Cooldown { get { return enableWC_Cooldown; } set { enableWC_Cooldown = value; } }
    public float ClimbingCounter { get { return climbingCounter; } set { climbingCounter = value; } }
    #endregion

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        playerInput = GetComponent<PlayerInput>();

        states = new StatesHandler(this);
        currentState = states.Grounded();
    }
    private void Start()
    {
        interaction_timer_counter = INTERACTION_TIMER;
        lastCheckpointLocation = this.transform.position;

        music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        music.volume = PlayerPrefs.GetFloat("MusicVolume");
        sfx.volume = PlayerPrefs.GetFloat("SoundFXVolume");


        rb2d = GetComponent<Rigidbody2D>();
        player_animation = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        MaxPlayerSpeed = WalkSpeed;
        originalGravityScale = rb2d.gravityScale;
    }

    private void Update()
    {
        isAutomaticWallClimbingOn = PlayerPrefs.GetString("AutomaticWallClimbing");
        currentState.UpdateStates();

        if (enableWC_Cooldown == true)
        {
            climbingCounter -= Time.deltaTime;
            if (climbingCounter <= 0)
            {
                enableWC_Cooldown = false;
            }
        }
        else { }


        if (checkInteraction == true)
        {
            interaction_timer_counter -= Time.deltaTime;
            if (interaction_timer_counter <= 0) { checkInteraction = false; interaction_timer_counter = INTERACTION_TIMER; }
        }
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateStates();

        if (onSpring() == true)
        {
            Debug.Log("spring activated");
            rb2d.AddForce(Vector2.up * SpringPower, ForceMode2D.Impulse);
        }
        else { };

        switch (isAutomaticWallClimbingOn)
            {
              case "True":
                AutomaticClimbingCooldown = 0.25f;
                if (enableWC_Cooldown == false)
                {
                    ClimbingPerformed();
                }
                else { };
                  break;
              case "False":
                  AutomaticClimbingCooldown = 0;
                  break;
            }
    }
    public void jumpCancel()
    {
        if (rb2d.velocity.y > 0f)
        {
            timeHoldingJump = 0;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y * 0.5f);
        }
    }


    // Collision Check
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            Death(lavaHurt, true);
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            Death(spikeHurt, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Crown"))
        {
            buttons.CrownCollected();
            // Add a win animation?
        }
        else { };

        if (collision.gameObject.CompareTag("Poison_Water"))
        {
            Death(lavaHurt, true); // change to poision hurt.
        }
    }

    // Input Manager
    #region Inputs
    public void activeJump() { jumpActivate = true; }
    public void Sprinting() 
    { 
        if (MaxPlayerSpeed == WalkSpeed)
        {
            MaxPlayerSpeed = PlayerSprint;
        }
        else if (MaxPlayerSpeed == PlayerSprint)
        {
            MaxPlayerSpeed = WalkSpeed;
        }
    }
    public void DashPerformed() { dashActivate = true; }
    public void ClimbingPerformed()
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
    }
    public void ClimbingCanceled()
    {
        enableWallClimbing = false;
    }

    public void Interact() { Debug.Log("will Interact!"); checkInteraction = true;}
    #endregion

    public void StartCountdown() { StopCoroutine(Cooldown()); StartCoroutine(Cooldown()); }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(BonusSpeedTime);
        bonusSpeedCounter = 0;
    }

    private void Death(AudioClip deathsfx, bool enableRandomPitch)
    {
        soundfxManager.PlaySFX(deathsfx, enableRandomPitch);
        this.transform.position = lastCheckpointLocation;
    }
}
