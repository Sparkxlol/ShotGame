using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] protected GameObject projTrail;
    [SerializeField] protected Transform projOrigin;
    [SerializeField] protected float projSpeed = 25f;
    [SerializeField] protected float projDis = 20f;
    [SerializeField] protected float projDamage = 5f;
    //[SerializeField] protected Animator muzzleAnim;

    protected bool firing = false;
    protected float curDirection = 1f;
    [SerializeField] protected bool automatic = true;
    [SerializeField] protected const float maxFireWait = .1f;
    protected float fireWait;

    [SerializeField] protected int maxBullets = 20;

    protected float firePause = 0; // Lower amount inbetween shots higher spread
    [SerializeField] protected float maxFirePause = Mathf.Infinity;
    protected int shotsSincePause; // Higher amount has more spread;
    [SerializeField] protected float maxShotsSincePause = Mathf.Infinity;
    [SerializeField] protected float fireSpreadPercent;

    private void Start()
    {
        if (maxShotsSincePause == Mathf.Infinity)
            maxShotsSincePause = maxBullets;

        if (maxFirePause == Mathf.Infinity)
            maxFirePause = maxFireWait * 5;
    }

    private void Update()
    {
        Fire();

        firePause += Time.deltaTime;
    }

    private void Fire()
    {
        if (firing && fireWait >= maxFireWait)
        {
            // muzzleAnim.SetTrigger("Shoot");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 sprayChange = FindSpread();
            Vector3 dirChange = mousePos - projOrigin.position;
            dirChange = new Vector3(dirChange.x + sprayChange.x / dirChange.x, dirChange.y + sprayChange.y / dirChange.y, dirChange.z);

            var hit = Physics2D.Raycast(
                projOrigin.position,
                dirChange,
                projDis);

            Debug.DrawRay(projOrigin.position, dirChange, Color.green, 20f);

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
                var endPos = (dirChange) * projDis;
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

    private Vector3 FindSpread()
    {
        if (firePause >= maxFirePause)
            shotsSincePause = 0;
        else
            shotsSincePause++;

        float maxDifference = 0;
        float firePauseDiff = firePause / maxFirePause;
        float shotPauseDiff = shotsSincePause / maxShotsSincePause;

        maxDifference += (firePauseDiff >= 1) ? 0 : 1 - firePauseDiff;
        maxDifference += (shotPauseDiff * 5) * fireSpreadPercent;

        firePause = 0;

        return new Vector3(Random.Range(-maxDifference, maxDifference), Random.Range(-maxDifference, maxDifference), 0);
    }
}
