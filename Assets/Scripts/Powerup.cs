using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private GameObject player;
    private CircleCollider2D circleCollider;
    private int healthBuff;
    private int healthTickBuff;


    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void InitializePowerup(GameObject obj, Sprite sprite, float rad, int buff, int tickBuff)
    {
        player = obj;
        healthBuff = buff;
        healthTickBuff = tickBuff;
        GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        GetComponent<CircleCollider2D>().radius = rad;
    }
    private void ApplyEffect()
    {
        if (player != null)
        {
            player.GetComponent<Health>().AddHealth(healthBuff);
            player.GetComponent<DrainWarmth>().DecreaseDec(healthTickBuff);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ApplyEffect();
            Destroy(gameObject);
        }
    }
}
