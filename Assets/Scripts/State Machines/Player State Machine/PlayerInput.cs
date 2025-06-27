using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerController playerInputs;
    [SerializeField] private PlayerHandler playerHandler;
    [SerializeField] private CutsceneManager cutSceneManager;
    [SerializeField] private bool usePlayerInputs;
    [SerializeField] private bool useDialogueInput;
    public bool UsePlayerInputs { get { return usePlayerInputs; } set { usePlayerInputs = value; } }
    public bool dialogueEnter;
    public Vector2 movement;

    private void Awake()
    {
        playerInputs = new PlayerController();

        if (playerInputs != null && usePlayerInputs == true)
        {
            playerInputs.Action.Pause.performed += enablepause => playerHandler.pausemenu_script.EnablePauseMenu();

            playerInputs.Action.Dash.performed += dash_performed => playerHandler.DashPerformed();

            playerInputs.Action.Jump.started += jumpactivating => playerHandler.activeJump();
            playerInputs.Action.Jump.canceled += jumpcancel => playerHandler.jumpCancel();

            playerInputs.Action.Climbing.performed += climbing_performed => playerHandler.ClimbingPerformed();
            playerInputs.Action.Climbing.canceled += exit_climb => playerHandler.ClimbingCanceled();

            playerInputs.Action.Interact.started += start_interaction => playerHandler.Interact();
        }
        else { };

        if (cutSceneManager != null)
        {
            playerInputs.Cutscene.Skip.started += skip_cutscene => cutSceneManager.CutsceneSkip();
        }
        else { }

        if (useDialogueInput == true)
        {
            playerInputs.Dialogue.Continue.started += dialogue_play => DialogueContinue();

            // Allow dialogue to be pushed forward.
        }
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
        if (playerInputs != null && usePlayerInputs == true)
        {
            movement = playerInputs.Action.Movement.ReadValue<Vector2>();
            if (movement.x > 0) { movement.x = Mathf.Ceil(movement.x); }
            else { movement.x = Mathf.FloorToInt(movement.x); }

            if (usePlayerInputs == true && playerHandler != null && playerHandler.EnableWallClimbing == true)
            {
                movement = playerInputs.Action.MoveUp.ReadValue<Vector2>();
            }
        }
        else if (playerInputs != null && usePlayerInputs == false)
        {
            movement.x = 0;
        }
    }

    private void DialogueContinue()
    {

    }
}
