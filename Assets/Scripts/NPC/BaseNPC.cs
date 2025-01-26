using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer interactionSprite;
    protected PlayerHandler playerHandler;
    private GameObject Player;
    private Vector3 playerPos;
    private bool triggerCheck;

    private const float INTERACTION_DISTANCE = 5;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        playerHandler = Player.GetComponent<PlayerHandler>();
    }
    private void Update()
    {

        playerPos = Player.transform.position;

        // omg hi future me if you want you can easilly turn this into a guard clause if you ever want to actually improve the code.
        if (playerHandler.checkInteraction == true && IsWithinInteractionDistance() == true && triggerCheck == false)
        {
            triggerCheck = true;
            Interact();
        }
        else { }

        if (IsWithinInteractionDistance() == true)
        {
            interactionSprite.gameObject.SetActive(true);
        }
        else if (IsWithinInteractionDistance() == false)
        {
            interactionSprite.gameObject.SetActive(false);
        }

        if(playerHandler.checkInteraction == false && triggerCheck == true) { triggerCheck = false; }
    }

    public abstract void Interact();

    private bool IsWithinInteractionDistance()
    {
        if (Vector2.Distance(playerPos, this.transform.position) < INTERACTION_DISTANCE)
        {
            return true;
        }
        else { return false; }
    }
}
