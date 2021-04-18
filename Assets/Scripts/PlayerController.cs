<<<<<<< HEAD
﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D             rb;
    [SerializeField] private float speed                = 3f;
    [SerializeField] private float jumpForce            = 15f;
    private bool                   isGrounded           = false;
    private bool                   onRight              = true;

    public Transform               groundCheck;
    public Transform               attackPos;

    private float                  timeBtwAttack;
    public float                   startTimeBtwAttack;
    public LayerMask               whatIsEnemies;
    public float                   attackRange;
    public int                     damage;
    public float                   attackForce;

    private SpriteRenderer         sprite;
    private Animator               animator;
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 15f;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    public Transform groundCheck;

    private SpriteRenderer sprite;
    private Animator animator;
>>>>>>> master

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        CheckGround();
        
    }
    void Update()
    {
        if (isGrounded)
            animator.SetInteger("State", 1);
    
        if (Input.GetButton("Horizontal"))
            Run();
        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.Mouse0) && timeBtwAttack <= 0)
            Attack();
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
=======
>>>>>>> master
    }

    private void Run()
    {
        if(isGrounded)
            animator.SetInteger("State", 2);
        sprite.flipX = Input.GetAxis("Horizontal") < 0.0f;
<<<<<<< HEAD
        onRight = Input.GetAxis("Horizontal") > 0.0f;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        attackPos.transform.position = (onRight) ? new Vector2(transform.position.x + 0.75f, transform.position.y)
                                                 : new Vector2(transform.position.x - 0.75f, transform.position.y);
=======
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
>>>>>>> master
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f);
        isGrounded = colliders.Length > 2;
    }

<<<<<<< HEAD
    private void Attack()
    {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; ++i)
            {
                enemiesToDamage[i].GetComponent<HealthBar>().GetDamage(damage);
            enemiesToDamage[i].GetComponent<Rigidbody2D>().AddForce(
               onRight? transform.right * attackForce: transform.right * -attackForce, ForceMode2D.Impulse);
            }
            timeBtwAttack = startTimeBtwAttack;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
=======
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
    }
    */
>>>>>>> master
}
