using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool hasSpawned;
    private PlayerHandler playerHandler;
    private BoxCollider2D boxCollider;
    private void Awake()
    {
        playerHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        StartUp();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        boxCollider.excludeLayers &= ~(1 << LayerMask.NameToLayer("Player"));
    }

    private void Update()
    {
        if (!hasSpawned)
        {
            StartUp();
        }
        else
        {
            if (playerHandler.activateDeathAnim == true)
            {
                boxCollider.excludeLayers = LayerMask.GetMask("Player");
                ObjectPoolManager.ReturnObjectToPool(gameObject);
                hasSpawned = false;
            }
        }
    }

    private int Randomize()
    {
        int a = Random.Range(0, 2);
        return a;
    }

    private void StartUp()
    {
        hasSpawned = true;
        float x = Random.Range(10, 20);
        if (Randomize() == 1) { x *= -1f; }
        float y = Random.Range(15, 30);
        rb2d.velocity = new Vector2(x, y);
    }
}
