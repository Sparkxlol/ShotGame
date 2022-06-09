using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private EnemyController mainEnemy;
    [SerializeField] private float multiplier = 1.0f;

    public void Hit(float damage)
    {
        mainEnemy.TakeDamage(damage * multiplier);
    }
}
