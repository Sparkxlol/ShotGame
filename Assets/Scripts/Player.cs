using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Sprite[] rifleSprites;
    [SerializeField] private GameObject weapon = null;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    [Header("Walking")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float runSpeedMult = 1.5f;
    [SerializeField] private float crouchSpeedMult = .5f;
    private float direction;
    private bool canMove = true;

    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        RotateTowardsMouse();
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
        /*
        Debug.Log(direction + " " + angle);
        if (weapon != null && ((angle >= -90 && direction == -1) || (angle < -90 && direction == 1)))
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;

            weapon.GetComponent<Gun>().FlipGun();
        }
        else*/ if (direction != 0 && direction != transform.localScale.x)
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


    private void RotateTowardsMouse()
    {
        // Check animation state.
        if (weapon == null)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 gunPos = weapon.transform.position;
        angle = -(Mathf.Atan2(gunPos.x - mousePos.x, gunPos.y - mousePos.y) * Mathf.Rad2Deg) - 90;
        weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        float newAngle = -angle;
        if (newAngle >= 234 || newAngle < -72)
            spriteRenderer.sprite = rifleSprites[4];
        else if (newAngle >= 198 || newAngle < -36)
            spriteRenderer.sprite = rifleSprites[3];
        else if (newAngle >= 162 || newAngle < 36)
            spriteRenderer.sprite = rifleSprites[2];
        else if (newAngle >= 126 || newAngle < 72)
            spriteRenderer.sprite = rifleSprites[1];
        else
            spriteRenderer.sprite = rifleSprites[0];
    }
}
