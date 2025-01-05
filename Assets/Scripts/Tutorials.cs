using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorials : MonoBehaviour
{
    public int TutorialID;

    public GameObject WallClimbing_1;
    public GameObject WallClimbing_2;
    public GameObject Dash;

    public PlayerHandler playerHandler;
    private bool PlayerWatchedWallClimbing;
    private bool PlayerWatchedDashing;


    public void NextTip()
    {
        WallClimbing_1.SetActive(false);
        WallClimbing_2.SetActive(true);
    }

    public void CloseTutorial()
    {
        WallClimbing_2.SetActive(false);
        playerHandler.enabled = true;
    }

    public void Thanks()
    {
        Dash.SetActive(false);
        playerHandler.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && PlayerWatchedWallClimbing == false && TutorialID == 1)
        {
            playerHandler.rb2d.velocity = Vector2.zero;
            PlayerWatchedWallClimbing = true;
            playerHandler.enabled = false;
            WallClimbing_1.SetActive(true);
        }

        if (collision.gameObject.CompareTag("Player") && PlayerWatchedDashing == false && TutorialID == 2)
        {
            playerHandler.rb2d.velocity = Vector2.zero;
            PlayerWatchedDashing = true;
            playerHandler.enabled = false;
            Dash.SetActive(true);
            // Set Game Object Active 
        }
    }
}
