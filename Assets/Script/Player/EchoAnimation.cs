using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoAnimation : MonoBehaviour
{
    public float spawnRate;
    public float startSpawnRate;

    public GameObject echo;
    Movement moving;

    private void Start()
    {
        moving = GetComponent<Movement>();
    }
    private void Update()
    {
        if (!moving.isDashing)
        {
            return;
        }

        if (spawnRate <= 0)
        {
            var instance = Instantiate(echo, transform.position, Quaternion.identity);
            instance.transform.localScale = gameObject.transform.localScale;
            Destroy(instance, 1f);
            spawnRate = startSpawnRate;
        }
        else
        {
            spawnRate -= Time.deltaTime;
        }
    }
}

