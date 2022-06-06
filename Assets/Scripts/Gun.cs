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
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hit = Physics2D.Raycast(
                projOrigin.position,
                mousePos - projOrigin.position,
                projDis);

            Debug.DrawRay(projOrigin.position, mousePos - projOrigin.position, Color.green, 20f);

            var trail = Instantiate(
                projTrail,
                projOrigin.position,
                Quaternion.identity);

            var trailScript = trail.GetComponent<TrailEffect>();

            if (hit.collider != null)
            {
                trailScript.SetTargetPosition(hit.point);
                if (hit.collider.transform.parent != null && hit.collider.transform.parent.CompareTag("Enemy"))
                    hit.collider.GetComponent<EnemyDamage>().Hit(projDamage);
            }
            else
            {
                var endPos = projOrigin.position + mousePos * projDis;
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
