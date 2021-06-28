using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    bool movingRight = true;
    float movementSpeed = 2f;
    float groundCheckRadius = .1f;
    Rigidbody2D rb;
    public Transform groundCheck;
    public Collider2D col;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask wall;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Patroling()
    {
        rb.velocity = new Vector2(transform.localScale.x * movementSpeed, 0);
        var groundinfo = !Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
        if (groundinfo || col.IsTouchingLayers(wall))
        {
            if (movingRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
                movingRight = false;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                movingRight = true;
            }
        }
    }
}
