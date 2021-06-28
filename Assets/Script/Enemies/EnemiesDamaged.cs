using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDamaged : MonoBehaviour
{
    public int currentHealth;
    Animator anim;
    public Collider2D col;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamaga(int damage, float hitDirection)
    {
        anim.SetTrigger("isHurt");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die(hitDirection);
        }
    }
    public void Die(float hitDirection)
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(hitDirection * 5, 5), ForceMode2D.Impulse);
        anim.SetBool("isDead", true);
        col.enabled = false;
        Destroy(gameObject, 1);
    }
}
