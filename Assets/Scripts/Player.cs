using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject weapon = null;
    private Rigidbody2D rb;

    [Header("Walking")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float runSpeedMult = 1.5f;
    [SerializeField] private float crouchSpeedMult = .5f;
    private float direction;
    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        Flip();

        Shoot();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Shoot()
    {
        if (weapon != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                weapon.GetComponent<Gun>().StartFire();
            else if (Input.GetKeyUp(KeyCode.Mouse0))
                weapon.GetComponent<Gun>().EndFire();
            else if (Input.GetKeyDown(KeyCode.R))
                weapon.GetComponent<Gun>().Reload();
        }
    }

    private void Flip()
    {
        if (direction != 0 && direction != transform.localScale.x)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;

            direction *= -1;

            weapon.GetComponent<Gun>().FlipGun();
        }
    }

    private void Move()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.LeftControl))
                rb.velocity = new Vector2(direction * speed * crouchSpeedMult, rb.velocity.y);
            else if (Input.GetKey(KeyCode.LeftShift))
                rb.velocity = new Vector2(direction * speed * runSpeedMult, rb.velocity.y);
            else
                rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }
    }
}
