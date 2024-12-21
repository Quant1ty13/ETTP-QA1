using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : EnemyStats
{
    float speed { get { return enemySpeed; } set { enemySpeed = value; } }
    float jumpHeight { get { return enemyJumpHeight; } set { enemyJumpHeight = value; } }
    public int Damage { get { return enemyDamage; } set { enemyDamage = value; } }
    public float enemyRange { get { return AgroRange; } set { AgroRange = value; } }

    bool Grounded = true;
    bool Jumping = false;
    bool movingRight = true;
    bool movingLeft = false;

    [SerializeField] GameObject pointA;
    private Vector2 targetA;
    [SerializeField] GameObject pointB;
    private Vector2 targetB;
    [SerializeField] GameObject playerPosition;
    private Vector2 currentPosition;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private PlayerMovement movementcheck;
    float PlayerDistance;

    private void Start()
    {
        targetA = pointA.transform.position;
        targetB = pointB.transform.position;
    }

    private void Update()
    {
        currentPosition = this.transform.position;
        PlayerDistance = Vector2.Distance(playerPosition.transform.position, currentPosition);
        MovementAI();

        if (PlayerDistance <= enemyRange && Grounded == true && Jumping == false)
        {
            Jump();
        }
    }

    protected override void MovementAI()
    {

        if (Vector2.Distance(currentPosition, targetB) >= 0.5f && movingRight == true)
        {
            transform.position = Vector2.MoveTowards(currentPosition, targetB, speed * Time.deltaTime);
        }
        else
        {
            movingRight = false;
            movingLeft = true;
        }

        if (Vector2.Distance(currentPosition, targetA) >= 0.5f && movingLeft == true)
        {
            transform.position = Vector2.MoveTowards(currentPosition, targetA, speed * Time.deltaTime);
        }
        else
        {
            movingLeft = false;
            movingRight = true;
        }
    }

    private void Jump()
    {
        Jumping = true;
        Debug.Log("enemy has jumped");
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Jumping = false;
        Grounded = true;

        if (collision.gameObject.CompareTag("Player")/* && movementcheck.Dashing == true*/)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Grounded = false;
    }
}
