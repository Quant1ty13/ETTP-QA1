using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerController playerInputs;
    [SerializeField] private PlayerHandler playerHandler;
    [SerializeField] private CutsceneManager cutSceneManager;
    public Vector2 movement;

    private void Awake()
    {
        playerInputs = new PlayerController();

        if (playerInputs != null)
        {
            playerInputs.Action.Pause.performed += enablepause => playerHandler.pausemenu_script.EnablePauseMenu();

            playerInputs.Action.Dash.performed += dash_performed => playerHandler.DashPerformed();

            playerInputs.Action.Jump.started += jumpactivating => playerHandler.activeJump();
            playerInputs.Action.Jump.canceled += jumpcancel => playerHandler.jumpCancel();

            playerInputs.Action.Sprint.started += sprinting => playerHandler.Sprinting();

            playerInputs.Action.Climbing.performed += climbing_performed => playerHandler.ClimbingPerformed();
            playerInputs.Action.Climbing.canceled += exit_climb => playerHandler.ClimbingCanceled();
        }
        else { };

        if (cutSceneManager != null)
        {
            playerInputs.Cutscene.Skip.started += skip_cutscene => cutSceneManager.ChangeScene();
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
        if (playerInputs != null)
        {
            movement = playerInputs.Action.Movement.ReadValue<Vector2>();
            if (movement.x > 0) { movement.x = Mathf.Ceil(movement.x); }
            else { movement.x = Mathf.FloorToInt(movement.x); }
        }
    }
}
