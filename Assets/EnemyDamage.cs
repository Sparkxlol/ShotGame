using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private EnemyController mainEnemy;
    [SerializeField] private float multiplier = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            mainEnemy.TakeDamage(other.GetComponent<Projectile>().GetBaseDamage() * multiplier);
        }
    }

}
