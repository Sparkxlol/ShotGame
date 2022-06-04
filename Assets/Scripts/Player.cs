using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed = 5f;
    private float direction;
    private bool canMove = true;
    [SerializeField] private GameObject weapon = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();
    }

    private void FixedUpdate()
    {
        if (canMove)
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    private void Shoot()
    {
        if (weapon != null)
            weapon.GetComponent<Gun>().Fire(direction);
    }
}
