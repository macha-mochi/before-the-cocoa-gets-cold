using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainWarmth : MonoBehaviour
{
    [SerializeField] int decrement;
    [SerializeField] int increment;
    [SerializeField] float tickSpeed;
    [SerializeField] Health player;
    public bool inFire;
    float timer;

    private void Start()
    {
        timer = tickSpeed;
    }
    
    public void DecreaseDec(int amount)
    {
        decrement -= amount;
    }
    void Update()
    {
        if (!inFire)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                player.TakeDamage(decrement);
                timer = tickSpeed;
            }

        }

        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                player.AddHealth(increment);
                timer = tickSpeed;
            }
        }
    }
}
