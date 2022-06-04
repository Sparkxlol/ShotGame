using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform projOrigin;
    [SerializeField] protected float projSpeed = 25f;

    public void Fire(float direction)
    {
        GameObject obj = Instantiate(projectile, projOrigin.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(projSpeed * direction, 0), ForceMode2D.Impulse);
    }
}
