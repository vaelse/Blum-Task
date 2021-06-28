using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    InputSystem input;
    Animator anim;
    int damage = 1;
    Movement Grounded;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;
    public float x = 2f;
    float y = 0f;

    private void Awake()
    {
        input = new InputSystem();
        anim = GetComponent<Animator>();
        Grounded = GetComponent<Movement>();
    }

    private void Update()
    {
        if(PauseMenu.isMenuActive)
        {
            return;
        }
        if (Time.time >= y && input.Player.Attack.triggered)
        {
            Attack();
            y = Time.time + (1f / x);
        }
    }

    public void Attack()
    {
        if (Grounded.isGrounded)
        {
            anim.SetTrigger("Attack");
            var enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (var enemy in enemiesHit)
            {
                enemy.GetComponent<EnemiesDamaged>().TakeDamaga(damage, transform.localScale.x);
            }
        }
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}
