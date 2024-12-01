using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : PlayerStat, DmgCalc
{
    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject Tutorial;
    [SerializeField] private PlayerMovement movementcheck;
    [SerializeField] private GameObject player;

    bool TutorialWatched = false;
    private void Update()
    {
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
            GameOver.SetActive(true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            TrapDamageScript trap = collision.gameObject.GetComponent<TrapDamageScript>();

            if (trap != null)
            {
                enemyDamageCalc(trap.trapDamage);
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();

            if (enemy != null && movementcheck.Dashing == false)
            {
                enemyDamageCalc(enemy.Damage);
            }
        }

        if (collision.gameObject.CompareTag("Win"))
        {
            WinScreen.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TutorialWatched == false)
        {
            Debug.Log("i am in a trigger");
            player.SetActive(false);
            Tutorial.SetActive(true);
            TutorialWatched = true;
        }
    }

    public void enemyDamageCalc(int dmg)
    {
        playerHealth -= dmg;
    }
}