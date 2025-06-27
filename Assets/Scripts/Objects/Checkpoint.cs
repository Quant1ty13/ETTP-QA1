using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Vector2 checkpointLocation;
    public PlayerHandler playerHandler;
    public AudioClip checkpoint;
    
    void Awake()
    {
        checkpointLocation = this.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHandler.lastCheckpointLocation != checkpointLocation)
            {
                playerHandler.soundfxManager.PlaySFX(checkpoint, true);
                playerHandler.lastCheckpointLocation = checkpointLocation;
            }
        }
    }
}
