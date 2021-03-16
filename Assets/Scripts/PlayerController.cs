using System.Collections;
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
    }

    private void Run()
    {
        if(isGrounded)
            animator.SetInteger("State", 2);
        sprite.flipX = Input.GetAxis("Horizontal") < 0.0f;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
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
}
