using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : PlayerStat
{
    [SerializeField] private GameObject GameOver;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            TrapDamageScript trap = collision.gameObject.GetComponent<TrapDamageScript>();

            if(trap != null)
            {
                playerHealth -= trap.trapDamage;

                if (playerHealth < 0)
                {
                    Destroy(gameObject);
                    GameOver.SetActive(true);
                }
            }
        }
    }
}