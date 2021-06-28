using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerKnockBack(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerKnockBack(collision);
    }

    public void PlayerKnockBack(object collision)
    {
        Collider2D col;
        if (collision.GetType() == typeof(Collision2D))
        {
             col = (Collider2D)collision.GetType().GetProperties().Single(x => x.Name == "collider").GetValue(collision, null);
        }
        else
        {
            col = (Collider2D)collision;
        }

        if (col.gameObject.tag == "Player")
        {
            var player = col.GetComponent<Movement>();
            var damagePlayer = col.GetComponent<PlayerDamaged>();
            damagePlayer.TakeDamaga(1);

            player.knockbackCount = player.knockbackLenght;

            if (col.transform.position.x < transform.position.x)
            {
                player.knockfromright = true;
            }
            else
            {
                player.knockfromright = false;
            }
        }
    }

}

