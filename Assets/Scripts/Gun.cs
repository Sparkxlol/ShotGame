using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform projOrigin;
    [SerializeField] protected float projSpeed = 25f;
    protected bool firing = false;
    protected float curDirection = 1f;
    [SerializeField] protected bool automatic = true;
    [SerializeField] protected const float maxFireWait = .1f;
    protected float fireWait;

    private void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (firing && fireWait >= maxFireWait)
        {
            // Creates a bullet and moves it in the correct direction.
            GameObject obj = Instantiate(projectile, projOrigin.position, Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(projSpeed * curDirection, 0), ForceMode2D.Impulse);

            fireWait = 0f;
            if (!automatic)
                EndFire();
        }
        fireWait += Time.deltaTime;
    }

    public void StartFire()
    {
        firing = true;
    }

    public void EndFire()
    {
        firing = false;
    }

    public void FlipGun()
    {
        curDirection *= -1;
    }
}
