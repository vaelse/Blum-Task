using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PlayerDamaged : MonoBehaviour
{
    Animator anim;
    public int currentHealth = 3;
    public int currentHeartCount;
    public Image[] hearts;
    public UnityEvent myUnityEvent;

    private void Awake()
    {
        if (myUnityEvent == null)
            myUnityEvent = new UnityEvent();
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        currentHeartCount = currentHealth;
    }

    public void TakeDamaga(int damage)
    {
        if (currentHealth <= currentHeartCount)
        {
            currentHeartCount--;
            hearts[currentHeartCount].GetComponent<Animator>().SetTrigger("isHit");
        }

        anim.SetTrigger("isHurt");
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            anim.SetBool("isDead", true);
           StartCoroutine(ExecuteAfterTime());
        }
    }

    private void OnDisable()
    {
        myUnityEvent.Invoke();
    }
    public void GainHealth()
    {
        currentHealth++;
        if (currentHealth > currentHeartCount)
        {
            currentHeartCount++;
            hearts[currentHeartCount - 1].GetComponent<Animator>().SetTrigger("gainHealth");
        }
    }
    IEnumerator ExecuteAfterTime()
    {
        GetComponent<Movement>().enabled = false;
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
    }
}
