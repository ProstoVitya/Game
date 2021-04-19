using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    [Header("Move Patameters")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;
    private float gravityScale = 5f;
    private bool isGrounded = false;
    private bool onRight = true;
    private bool inCollWLadder = false;
    public Transform groundCheck;
    private Vector2 moveVector;
    [Header("Attack Patameters")]
    public Transform attackPos;
    public LayerMask Enemies;
    private bool isAttacking = false;
    public float attackRange;
    public int damage;
    public float attackForce;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Horizontal") && !isAttacking)
            Run();
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
    }
    void Update()
    {
        CheckGround();
        if (Input.GetButtonDown("Fire1")&&!isAttacking&&isGrounded)
            Attack();
        if (isGrounded && !isAttacking)
        {
            animator.SetInteger("State", 1);
            if (Input.GetButton("Horizontal"))
                animator.SetInteger("State", 2);
            if (Input.GetButtonDown("Jump") && isGrounded)
                Jump();
        }

    }

    private void Run()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
        if (moveVector.x < 0 && onRight)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            onRight = false;
        }
        else if (moveVector.x > 0 && !onRight)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            onRight = true;
        }
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f);
        isGrounded = (colliders.Length > 2) && !inCollWLadder;
        if (!isGrounded&&!inCollWLadder)
            animator.SetInteger("State", 3);
    }
    private void Jump()
    {
     
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    
    private void Attack()
    {
        isAttacking = true;
        animator.SetInteger("State", 5);

        StartCoroutine(AttackTime());
    }
    private void onAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Enemies);
        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].GetComponent<HealthBar>().GetDamage(damage);
            colliders[i].GetComponent<Rigidbody2D>().AddForce(transform.right * attackForce, ForceMode2D.Impulse);
        }
    }
    private IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            rb.gravityScale = 0;
            inCollWLadder = true;
            animator.SetInteger("State", 4);
            moveVector.y = Input.GetAxisRaw("Vertical");
            animator.SetFloat("moveY", Mathf.Abs(moveVector.y));
            rb.velocity = new Vector2(rb.velocity.x, moveVector.y * verticalSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        rb.gravityScale = gravityScale;
        inCollWLadder = false;
    }

}/* private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            if (Input.GetButton("Vertical"))
            {
                onLadder = true;
                animator.SetInteger("State", 4);                
            }
            moveVector.y = Input.GetAxisRaw("Vertical");
            animator.SetFloat("moveY", Mathf.Abs(moveVector.y));
            if (onLadder)
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x, moveVector.y * verticalSpeed);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder")&&onLadder)
        {
            rb.gravityScale = gravityScale;
            onLadder = false;
        }
    }*/