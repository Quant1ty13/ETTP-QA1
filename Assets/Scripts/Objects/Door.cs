using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int DoorID;
    [SerializeField] private PlayerHandler playerHandler;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerHandler.KeyList.Contains(DoorID))
        {
            Destroy(gameObject);
        }
    }
}
