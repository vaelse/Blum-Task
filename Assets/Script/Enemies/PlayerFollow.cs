using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    bool isFacingLeft;
    public GameObject player;
    public float detectRange;
    public float movementSpeed;
    Animator anim;
    Rigidbody2D rb;
    public LayerMask Ground;
    public bool isGrounded;
    public Collider2D col;
    public bool isPatrolling;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        isGrounded = col.IsTouchingLayers(Ground);
        if (isGrounded)
        {
            var distanceDif = Vector2.Distance(transform.position, player.transform.position);
            if (distanceDif < detectRange && player.GetComponent<PlayerDamaged>().currentHealth > 0)
            {
                Chasing();
                if (distanceDif < 1)
                {
                    anim.SetTrigger("Attack");
                    rb.velocity = new Vector2(0, 0);
                }
            }
            else if(isPatrolling)
            {
                anim.SetBool("isMoving", true);
                gameObject.GetComponent<Patrol>().Patroling();
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
    }

    public void Chasing()
    {
        Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime);
        anim.SetBool("isMoving", true);
        if (transform.position.x < player.transform.position.x)
        {
            rb.velocity = new Vector2(movementSpeed, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            rb.velocity = new Vector2(-movementSpeed, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

}