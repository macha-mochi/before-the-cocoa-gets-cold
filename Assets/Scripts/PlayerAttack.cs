using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Vector3[] attackPositions;
    [SerializeField] private PlayerController controller;
    
    [SerializeField] private float animLength;
    [SerializeField] private int strength;
    [SerializeField] private float kbStrength;
    [SerializeField] private AudioSource hit;

    private Animator anim;
    [HideInInspector]
    public bool isAttacking;
    private float timer;
    [SerializeField] LayerMask enemyLayer;
    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }
    public void Attack(int dir)
    {
        
        switch (dir)
        {
            case 0:
                Collider2D colid = Physics2D.OverlapCircle(transform.position + transform.up, 1f,enemyLayer);
                if(colid != null)
                {
                    colid.GetComponent<Health>().TakeDamage(strength);
                    if (colid != null) StartCoroutine(knockback(transform.up, colid));
                    hit.Play();
                }
                break;
            case 1:
                colid = Physics2D.OverlapCircle(transform.position - transform.up, 1f, enemyLayer);
                if (colid != null)
                {
                    colid.GetComponent<Health>().TakeDamage(strength);
                    if (colid != null) StartCoroutine(knockback(-transform.up, colid));
                    hit.Play();
                }
                break;
            case 2:
                colid = Physics2D.OverlapCircle(transform.position + transform.right, 1f, enemyLayer);
                if (colid != null)
                {
                    colid.GetComponent<Health>().TakeDamage(strength);
                    if (colid != null) StartCoroutine(knockback(transform.right, colid));
                    hit.Play();
                }
                break;
            case 3:
                colid = Physics2D.OverlapCircle(transform.position - transform.right, 1f, enemyLayer);
                if (colid != null)
                {
                    colid.GetComponent<Health>().TakeDamage(strength);
                    if (colid != null) StartCoroutine(knockback(-transform.right, colid));
                    hit.Play();
                }
                break;
                
        }
        
        timer = animLength;
        isAttacking = true;
        controller.canMove = false;
    }

    private IEnumerator knockback(Vector2 dir, Collider2D colid)
    {
        colid.GetComponent<Enemy>().enabled = false;
        Time.timeScale = 0.01f;
        yield return new WaitForSeconds(0.0005f);
        Time.timeScale = 1;
        colid.GetComponent<Rigidbody2D>().AddForce(dir * kbStrength);
        yield return new WaitForSeconds(0.15f);
        colid.GetComponent<Enemy>().enabled = true;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && isAttacking)
        {
            isAttacking = false;
            controller.canMove = true;
        }
    }

}
