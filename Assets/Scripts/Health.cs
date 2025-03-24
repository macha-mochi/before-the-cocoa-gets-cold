using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private bool isTemp;
    [SerializeField] private float lifeSpan;
    [SerializeField] private AudioSource deathVFX;
    [SerializeField] private ParticleSystem deathSFX;
    [SerializeField] private GameObject gameController;
    [SerializeField] private GameObject present;
    [SerializeField] private bool dropsPresent;
    [SerializeField] private bool dropsBlanket;
    [SerializeField] private int dropRandomRange;
    [SerializeField] private float delay;
    private const int HOT_CHOCO = 0;
    private const int BLANKET = 1;
    public bool isDead = false;
    public int health;
    public int maxHealth;

    private void Awake()
    {
        health = maxHealth;
        if (isTemp)
        {
            StartCoroutine(lifeSpanCount());
        }
    }
    private IEnumerator lifeSpanCount()
    {
        yield return new WaitForSeconds(lifeSpan);
        Die();
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0 && !isDead)
        {
            health = 0;
            Die();
        }
    }

    public void AddHealth(int amount)
    {
        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Die()
    {
        if (gameObject.tag == "Player")
        {
            isDead = true;
            gameController.GetComponent<GameController>().ShowDeathScreen(2);
        }
        else
        {
            Enemy e = GetComponent<Enemy>();
            SpriteRenderer s = GetComponentInChildren<SpriteRenderer>();
            CircleCollider2D c = GetComponent<CircleCollider2D>();
            if (e != null)
            {
                e.enabled = false;
            }
            if (s != null)
            {
                s.enabled = false;
            }
            if (c != null)
            {
                c.enabled = false;
            }
            if (dropsPresent)
            {
                GameObject obj = Instantiate(present);
                obj.GetComponent<Present>().RandomizeType();
                obj.transform.position = gameObject.transform.position;
            }
            if (dropsBlanket)
            {
                GameObject.Find("Game Manager").GetComponent<GameController>().SpawnPowerup(BLANKET, gameObject.transform.position);
            }
            if (deathSFX != null) deathSFX.Play();
            if (deathVFX != null) deathVFX.Play();
            isDead = true;
            Destroy(gameObject, delay);

        }
    }
}
