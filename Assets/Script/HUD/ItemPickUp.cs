using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPickUp : MonoBehaviour
{
    Animator anim;
    int coin = 0;
    public TextMeshProUGUI text;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Coin")
        {
            collision.GetComponent<Animator>().SetTrigger("isPicked");
            coin++;
            text.text = "Coins " + coin;
            collision.GetComponent<Collider2D>().enabled = false;
        }
       else if (collision.gameObject.tag == "Food" && GetComponent<PlayerDamaged>().currentHealth < 3)
        {
            GetComponent<PlayerDamaged>().GainHealth();
            collision.gameObject.SetActive(false);
        }
    }
}
