using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
public class PlayerHandler : MonoBehaviour
{
    protected PlayerInput playerInput;

    [Header("Jumping")]
    public float JumpHeight;
    public float JumpBufferTime;
    public float CoyoteTime;
    public float ApexHangTime;
    public float jumpBonusSpeedTime;
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
    public float AccelerationRate;
    public float DecelerationRate;
    public float DashShakeForce;
    public float DashSpeed;
    public float DashTime;
    public float BonusHeight_Dash;
    public float BonusSpeed_Dash;
    public float dashBonusSpeedTime;
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
    public ParticleSystem moveParticle;
    public ParticleSystem deathParticle;
    public ParticleSystem flameParticle;
    public GameObject dashOverlay;
    public Animator dash_animation;
    private bool flameParticlePlayed = false;

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
    public BaseAmbience baseAmbience;
    public AudioClip[] footsteps;
    public AudioClip[] fall;
    public AudioClip[] enterClimb;
    public AudioClip[] climbingUp;
    public AudioClip[] jump;
    public AudioClip dash;
    public AudioClip spikeHurt;
    public AudioClip lavaHurt;
    public PauseMenu pausemenu_script;
    public AudioSource music;
    public AudioSource sfx;
    [SerializeField] private AudioClip spring_sfx;

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
    public GameObject BL_Raycast;
    public GameObject ML_Raycast;
    public GameObject BR_Raycast;
    public GameObject MR_Raycast;
    public GameObject GroundTrigger;
    public Image BlackScreen;
    public float originalGravityScale;
    private string isAutomaticWallClimbingOn;
    private const float INTERACTION_TIMER = 0.1f;
    private const float JUSTFALLEN_TIMER = 0.3f;
    private float justfallen_timer_counter;
    private float interaction_timer_counter;
    public List<int> KeyList = new List<int>();
    public bool onGround() { return Physics2D.OverlapCircle(groundCheck.position, 0.25f, defineGround); }
    public bool onGround_Climb() { return Physics2D.OverlapCircle(groundCheck.position, 0.25f, defineClimbableWall); }
    public bool onSpring() { return Physics2D.OverlapCircle(groundCheck.position, 0.33f, defineSprings); }
    public Vector2 lastCheckpointLocation;
    public bool enableClimbJump { get; private set; }
    public bool EnableClimbJump { get { return enableClimbJump; } set {  enableClimbJump = value; } }
    public bool jumpQueue { get; private set; }
    public bool JumpQueue { get { return jumpQueue; } set { jumpQueue = value; } }
    public bool enableCoyoteDashJump { get; private set; }
    public bool EnableCoyoteDashJump { get { return enableCoyoteDashJump; } set { enableCoyoteDashJump = value; } }
    public bool justFallen { get; private set; }
    public bool checkInteraction { get; private set; }
    public bool CheckInteraction { get { return checkInteraction; } set { CheckInteraction = value; } }
    public bool gameConcluded { get; private set; }
    public bool GameConcluded { get { return gameConcluded; } set { gameConcluded = value; } }
    public CinemachineImpulseSource impulseSource { get; private set; }

    // Death Animation
    public bool activateDeathAnim;
    private bool disappearPlayer;
    private const float PLAYER_DISAPPEAR_TIME = 0.2f;
    private float disappearCounter;
    private const float FADE_BLACK_TIME = 0.5f;
    private float fadeTB_Counter;
    private float alphaCounter_PLAYER = 1;
    private float alphaCounter_BS = 0;
    private bool fadeIn;
    private float fadeIn_Counter;
    private const float FADE_IN_TIME = 0.5f;
    private float waitCounter = 0.15f;

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
    public bool JustFallen { get { return justFallen; } set { justFallen = value; } }

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
        soundfxManager.ambience = baseAmbience;

        impulseSource = GetComponent<CinemachineImpulseSource>();

        playerInput = GetComponent<PlayerInput>();

        states = new StatesHandler(this);
        currentState = states.Grounded();
    }
    private void Start()
    {
        interaction_timer_counter = INTERACTION_TIMER;
        justfallen_timer_counter = JUSTFALLEN_TIMER;
        lastCheckpointLocation = this.transform.position;

        music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        music.volume = PlayerPrefs.GetFloat("MusicVolume");
        sfx.volume = PlayerPrefs.GetFloat("SoundFXVolume");


        rb2d = GetComponent<Rigidbody2D>();
        player_animation = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        MaxPlayerSpeed = WalkSpeed;
        originalGravityScale = rb2d.gravityScale;

        disappearCounter = PLAYER_DISAPPEAR_TIME;
        fadeTB_Counter = FADE_BLACK_TIME;
        fadeIn_Counter = FADE_IN_TIME;
    }

    private void Update()
    {

        #region DeathAnim
        if (activateDeathAnim == true)
        {
            playerInput.UsePlayerInputs = false;

            // Make player turn to Zero Opacity
            if (disappearPlayer == false)
            {
                rb2d.velocity = new Vector2(0, 0);
                alphaCounter_PLAYER -= 0.08f;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alphaCounter_PLAYER);

                disappearCounter -= Time.deltaTime;

                if (disappearCounter <= 0)
                {
                    disappearPlayer = true;
                    disappearCounter = PLAYER_DISAPPEAR_TIME;
                    alphaCounter_PLAYER = 1;
                }
            }

            // Fade To Black
            if (disappearPlayer == true && fadeIn == false)
            {
                //alphaCounter_BS += 0.04f;
                alphaCounter_BS += (2.7f * Time.deltaTime);
                BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, alphaCounter_BS);

                fadeTB_Counter -= Time.deltaTime;

                if (fadeTB_Counter <= 0)
                {
                    alphaCounter_BS = 1;
                    this.transform.position = lastCheckpointLocation;
                    rb2d.gravityScale = originalGravityScale;
                    fadeIn = true;
                    fadeTB_Counter = FADE_BLACK_TIME;
                }
            }

            // Fade Black Out
            if (fadeIn == true)
            {
                if (waitCounter >= 0)
                {
                    waitCounter -= Time.deltaTime;
                    return;
                }

                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alphaCounter_PLAYER);

                //alphaCounter_BS -= 0.04f;
                alphaCounter_BS -= (2.7f * Time.deltaTime);
                BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, alphaCounter_BS);

                fadeIn_Counter -= Time.deltaTime;

                if (fadeIn_Counter <= 0.1)
                {
                    playerInput.UsePlayerInputs = true;
                }

                if (fadeIn_Counter <= 0)
                {
                    alphaCounter_BS = 0;
                    activateDeathAnim = false;
                    waitCounter = 0.1f;
                    disappearPlayer = false;
                    fadeIn = false;
                    fadeIn_Counter = FADE_IN_TIME;
                }
            }
        }
        #endregion

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

        if (JustFallen == true)
        {
            justfallen_timer_counter -= Time.deltaTime;
            if (justfallen_timer_counter <= 0) { JustFallen = false; justfallen_timer_counter = JUSTFALLEN_TIMER; }
        }

        if (hasDashed == true && flameParticlePlayed == false)
        {
            Debug.Log("playing flame particle");
            flameParticlePlayed = true;
            //flameParticle.Emit(1); works but ugly as shit
            flameParticle.Play();
        }
        else if(flameParticlePlayed == true && hasDashed  == false)
        {
            flameParticlePlayed = false;
            flameParticle.Stop();
        }
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateStates();

        if (onSpring() == true)
        {
            Debug.Log("spring activated");
            soundfxManager.PlaySFX(spring_sfx, true);
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
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
        jumpActivate = false;

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
    public void activeJump()
    { 
        if (!CheckForPlayerInput())
        {
            return;
        }

        jumpActivate = true; 
    }
    public void DashPerformed()
    {
        if (!CheckForPlayerInput())
        {
            return;
        }

        dashActivate = true; 
    }
    public void ClimbingPerformed()
    {
        if (!CheckForPlayerInput())
        {
            return;
        }

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

    public void Interact() { Debug.Log("will Interact!"); checkInteraction = true; }
    #endregion

    public void StartCountdown(float cooldown) { StopCoroutine(Cooldown(cooldown)); StartCoroutine(Cooldown(cooldown)); }

    public void EnableDashJumpAfterFall() { StartCoroutine(DisableDashCoyoteJump()); }

    private IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        bonusSpeedCounter -= BonusSpeed_Dash;
        if (bonusSpeedCounter <= BonusSpeed_Dash)
        {
            bonusSpeedCounter = 0;
        }
    }

    private IEnumerator DisableDashCoyoteJump()
    {
        yield return new WaitForSeconds(DashJumpGracePeriod);
        enableCoyoteDashJump = false;
    }

    private void Death(AudioClip deathsfx, bool enableRandomPitch)
    {
        if (activateDeathAnim == true)
        {
            return;
        }

        soundfxManager.PlaySFX(deathsfx, enableRandomPitch);
        impulseSource.GenerateImpulseWithForce(3);
        deathParticle.Play();
        playerInput.UsePlayerInputs = true;
        activateDeathAnim = true;
        rb2d.gravityScale = 0f;
        rb2d.velocity = new Vector2(0, 0);
    }

    private bool CheckForPlayerInput()
    {
        if (playerInput.UsePlayerInputs)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
