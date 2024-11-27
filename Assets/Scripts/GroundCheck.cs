using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] PlayerMovement movement;
    public SpriteRenderer spriteRender;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        if (normal == Vector2.up)
        {
            movement.hasJumped = false;
            movement.TR.emitting = false;
        }
    }
}
