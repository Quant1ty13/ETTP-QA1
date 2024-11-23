using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrap : TrapDamageScript
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.playerHealth -= trapDamage;
            }
        }
    }
}
