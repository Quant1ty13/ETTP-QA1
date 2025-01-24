using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private int KeyID;
    private bool KeyCollected;
    [SerializeField] private PlayerHandler Player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (KeyCollected == false && collision.CompareTag("Player"))
        {
            KeyCollected = true;
            Player.KeyList.Add(KeyID);
            Destroy(gameObject);
        }
    }
}
