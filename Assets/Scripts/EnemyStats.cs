using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStats : MonoBehaviour
{
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float enemyJumpHeight;
    [SerializeField] protected float AgroRange;
    [SerializeField] protected int enemyDamage;

    protected virtual void MovementAI()
    {

    }
}
