using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PlayerController.cs:
 * - This is used to control the player
 * - Inherits from Humanoid class
 */
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]

public class PlayerController : MonoBehaviour
{
    public static PlayerController controller;
    public enum Direction { UP, DOWN, RIGHT, LEFT };
    private Direction playerDirection;

    [SerializeField]
    PlayerAttack atk;

    [SerializeField]
    Health health;

    [SerializeField]
    string playerName;
    

    [SerializeField]
    float playerSpeed;

    [SerializeField]
    float attackCooldown;

    [SerializeField]
    Sprite[] spriteDirections;

    [SerializeField]
    private AudioSource attack;
    [SerializeField]
    private AudioSource walk;

    private Rigidbody2D      rb;
    private Animator         anim;
    private CircleCollider2D circCollider;
    private SpriteRenderer sr;
    private PlayerAttack playerAttack;
    private HealthBar healthBar;

    private bool isWalking = false;
    private bool isAttacking = false;
    public bool canMove = true;
    private float timer;


    private Vector2 movementVector;
    void Awake()
    {
        if (controller == null) controller = this;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circCollider = GetComponent<CircleCollider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        playerAttack = GetComponentInChildren<PlayerAttack>();
        healthBar = GetComponent<HealthBar>();
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.X) && timer <= 0)
        {
            anim.enabled = true;
            attack.Play();
            if (playerDirection == Direction.UP)
            {
                atk.Attack(0);
            }
            else if (playerDirection == Direction.DOWN)
            {
                atk.Attack(1);
            }
            else if (playerDirection == Direction.RIGHT)
            {
                atk.Attack(2);
            }
            else if (playerDirection == Direction.LEFT)
            {
                atk.Attack(3);
            }
            timer = attackCooldown;
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }
    private void FixedUpdate()
    {
        Animate();
        Move();
    }
    protected void Move()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");

        if (canMove)
        {
            rb.velocity = movementVector.normalized * playerSpeed;
            if (movementVector != Vector2.zero && !walk.isPlaying) walk.Play();
            else if (movementVector == Vector2.zero && walk.isPlaying)walk.Stop();
        }
        else
        {
            rb.velocity = Vector2.zero;
            walk.Stop();
        }
    }
    private void Animate()
    {
        if (playerAttack.isAttacking)
        {
            switch (playerDirection)
            {
                case Direction.RIGHT:
                    anim.SetFloat("Blend", 3);
                    break;
                case Direction.LEFT:
                    anim.SetFloat("Blend", 2);
                    break;
                case Direction.UP:
                    anim.SetFloat("Blend", 0);
                    break;
                case Direction.DOWN:
                    anim.SetFloat("Blend", 1);
                    break;
            }
        }
        
        else
        {
            if (movementVector.x > 0)
            {
                anim.SetFloat("Blend", 7);
                playerDirection = Direction.RIGHT;
            }
            else if (movementVector.x < 0)
            {
                anim.SetFloat("Blend", 6);
                playerDirection = Direction.LEFT;
            }
            else if (movementVector.y > 0)
            {
                anim.SetFloat("Blend", 4);
                playerDirection = Direction.UP;
            }
            else if (movementVector.y < 0)
            {
                anim.SetFloat("Blend", 5);
                playerDirection = Direction.DOWN;
            }
            else
            {
                switch (playerDirection)
                {
                    case Direction.RIGHT:
                        anim.SetFloat("Blend", 11);
                        break;
                    case Direction.LEFT:
                        anim.SetFloat("Blend", 10);
                        break;
                    case Direction.UP:
                        anim.SetFloat("Blend", 8);
                        break;
                    case Direction.DOWN:
                        anim.SetFloat("Blend", 9);
                        break;
                }
            }
        }
    }
    private void SetSpriteDirection()
    {
        if (!playerAttack.isAttacking)
        {
            anim.enabled = false;
        }
        switch (playerDirection)
        {
            case Direction.RIGHT:
                sr.sprite = spriteDirections[1];
                break;
            case Direction.LEFT:
                sr.sprite = spriteDirections[0];
                break;
            case Direction.UP:
                sr.sprite = spriteDirections[2];
                break;
            case Direction.DOWN:
                sr.sprite = spriteDirections[3];
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Projectile")
        {
            health.TakeDamage(collision.GetComponent<Projectile>().damage);
            if(collision.GetComponent<Health>() != null) collision.GetComponent<Health>().Die();
        }
    }
}
