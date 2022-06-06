using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] protected GameObject projTrail;
    [SerializeField] protected Transform projOrigin;
    [SerializeField] protected float projSpeed = 25f;
    [SerializeField] protected float projDis = 50f;
    [SerializeField] protected float projDamage = 5f;
    //[SerializeField] protected Animator muzzleAnim;

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
            // muzzleAnim.SetTrigger("Shoot");

            var hit = Physics2D.Raycast(
                projOrigin.position,
                transform.right,
                projDis);

            var trail = Instantiate(
                projTrail,
                projOrigin.position,
                transform.rotation);

            // Debug.DrawRay(projOrigin.position, transform.right * projDis, Color.green, 50f);

            var trailScript = trail.GetComponent<TrailEffect>();

            if (hit.collider != null)
            {
                trailScript.SetTargetPosition(hit.point);
                if (hit.collider.transform.parent.CompareTag("Enemy"))
                    hit.collider.GetComponent<EnemyDamage>().Hit(projDamage);
            }
            else
            {
                var endPos = projOrigin.position + transform.right * projDis;
                trailScript.SetTargetPosition(endPos);
            }

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
