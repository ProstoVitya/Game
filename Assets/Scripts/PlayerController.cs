using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    [Header("Move Patameters")]
    [SerializeField] private float speed         = 3f;
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float jumpForce     = 15f;
    private float                  gravityScale  = 5f;
    private bool                   isGrounded    = false;
    private bool                   onRight       = true;
    private bool                   inCollWLadder = false;
    public Transform               groundCheck;
    private Vector2                moveVector;

    [Header("Attack Patameters")]
    public Transform               attackPos;
    public LayerMask               Enemies;
    public LayerMask               Ground;
    private bool                   isAttacking   = false;
    public float                   attackRange;
    public int                     damage;
    public float                   attackForce;

    [Header("Inventory")]
    private int                    potionsCount  = 3;
    public Animator                potionAnim;
    private int                    weaponsCount  = 1;
    public Animator                weaponAnim;
    public bool                    hasKey        = false;
    public Animator                keyAnim;

    private Rigidbody2D            rb;
    private SpriteRenderer         sprite;
    private Animator               animator;

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
        if (Input.GetButtonDown("Fire1") && !isAttacking && isGrounded)
            Attack();
        if (isGrounded && !isAttacking)
        {
            animator.SetInteger("State", 1);
            if (Input.GetButton("Horizontal"))
                animator.SetInteger("State", 2);
            if (Input.GetButtonDown("Jump") && isGrounded)
                Jump();
        }
        if (Input.GetKeyDown(KeyCode.Q) && potionsCount > 0 /*&& хп < max*/) { 
            potionAnim.SetInteger("Count", --potionsCount);
            //увеличить хп
        }
        if (Input.GetButtonDown("Fire2") && weaponsCount > 0)
        {
            weaponAnim.SetInteger("weaponsCount", --weaponsCount);
            //выстрелить
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f,Ground);
        isGrounded = (colliders.Length > 0) && !inCollWLadder;
        if (!isGrounded && !inCollWLadder)
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
            if(!colliders[i].isTrigger)
            colliders[i].GetComponent<EnemyPatrol>().GetDamage(damage);
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
        if (collision.gameObject.CompareTag("Ladder") || collision.gameObject.CompareTag("Ladder_l"))
        {
            rb.gravityScale = 0;
            inCollWLadder = true;
            animator.SetInteger("State", 4);
            moveVector.y = Input.GetAxisRaw("Vertical");
            animator.SetFloat("moveY", Mathf.Abs(moveVector.y));
            rb.velocity = new Vector2(rb.velocity.x, moveVector.y * verticalSpeed);
        }

        if (collision.CompareTag("HealthPotion") && potionsCount < 3)
        {
            potionAnim.SetInteger("Count", ++potionsCount);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Weapon") && weaponsCount < 3)
        {
            weaponAnim.SetInteger("WeaponsCount", ++weaponsCount);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Key"))
        {
            hasKey = true;
            keyAnim.SetBool("hasKey", true);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        rb.gravityScale = gravityScale;
        inCollWLadder = false;
    }

}