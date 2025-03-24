using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    [SerializeField] float fireRad;
    [SerializeField] float tickSpeed;
    [SerializeField] int tickAmount;
    private CircleCollider2D circleCollider2D;
    private bool inRange;

    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = fireRad;
        inRange = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inRange = true;
            collision.GetComponent<DrainWarmth>().inFire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {   
        if (collision.tag == "Player")
        {
            inRange = false;
            collision.GetComponent<DrainWarmth>().inFire = false;
        }
    }
}
