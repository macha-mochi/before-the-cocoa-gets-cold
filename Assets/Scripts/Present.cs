using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{
    [SerializeField] private Sprite[] presentTypes;
    public GameObject player;
    private CircleCollider2D circleCollider;
    private AudioSource sfx;

    void Start()
    {
        sfx = GameObject.Find("Present Pickup").GetComponent<AudioSource>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void ApplyEffect()
    {
        if (player != null)
        {
            sfx.Play();
            int p = GameObject.Find("Game Manager").GetComponent<GameController>().incrementPresents(1);

        }
    }

    public void RandomizeType()
    {
        int type = Random.Range(0, presentTypes.Length);
        GetComponent<SpriteRenderer>().sprite = presentTypes[type];
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

