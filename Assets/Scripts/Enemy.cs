using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const int SNOWMAN = 0;
    private const int SNOWDOG = 1;
    [SerializeField] private float speed;
    private Transform target;
    [SerializeField] private int damage;
    [SerializeField] private float lineOfSight;
    [SerializeField] private float stopDistance;
    [SerializeField] private float attackRange;
    [SerializeField] private float hideRange;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int mobType;
    [SerializeField] private GameObject snowball;
    [SerializeField] private float snowballSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float appearSpeed;
    [SerializeField] private float attackTime;
    [SerializeField]private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;
    private bool isHidden = false;
    private bool isAppearing = false;
    private bool isAttacking = false;
    private float timer;
    private float appearTimer;
    private float attackTimer = 0f;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (mobType == SNOWDOG)
        {
            GetComponentInChildren<Projectile>().damage = damage;
            circleCollider.enabled = false;
            appearTimer = appearSpeed;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            isHidden = true;
        }
        else
        {
            circleCollider = GetComponent<CircleCollider2D>();
        }
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        float dist = (transform.position - target.position).magnitude;
        if (mobType == SNOWDOG && isHidden && dist < hideRange)
        {
            isAppearing = true;
        }
        if (mobType == SNOWDOG && circleCollider.enabled)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                circleCollider.enabled = false;
                attackTimer = attackTime;
            }
        }
        if (isAppearing)
        {
            appearTimer -= Time.deltaTime;
            float opacity = (appearSpeed - appearTimer) / appearSpeed;
            spriteRenderer.color = new Color(1f, 1f, 1f, opacity);
            if (appearTimer <= 0)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                isAppearing = false;
                isHidden = false;
            }
        }
        Vector2 dir = (transform.position - target.position).normalized;
        if (!isHidden)
        {
            if (dist < lineOfSight && dist > stopDistance)
            {
                rb.velocity = -dir * speed;
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
        }
        if (dist < attackRange && (mobType == SNOWMAN || mobType == SNOWDOG))
        {
            if (timer <= 0)
            {
                if (mobType == SNOWMAN)
                {
                    FireProjectile(dir);
                }
                if (mobType == SNOWDOG)
                {
                    attackTimer = attackTime;
                    circleCollider.enabled = true;
                    GetComponent<Animator>().SetTrigger("Appear");
                }
                timer = attackSpeed;
            }
        }
    }

    private void FireProjectile(Vector2 dir)
    {
        GameObject curr = Instantiate(snowball);
        curr.transform.position = transform.position;
        Rigidbody2D rig = curr.GetComponent<Rigidbody2D>();
        rig.velocity = snowballSpeed * -dir;
        curr.GetComponent<Projectile>().damage = damage;
    }

}
