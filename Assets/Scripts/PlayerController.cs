using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D            rb;

    [Header("Move Patameters")]
    [SerializeField] private float speed                = 3f;
    [SerializeField] private float verticalSpeed        = 5f;
    [SerializeField] private float jumpForce            = 15f;
    private float                  gravityScale         = 5f;
    private bool                   isGrounded           = false;
    private bool                   onRight              = true;
    private bool                   inCollWLadder        = false;
    public Transform               groundCheck;

    [Header("Attack Patameters")]
    public Transform               attackPos;
    public LayerMask               whatIsEnemies;
    private float                  timeBtwAttack;
    public float                   startTimeBtwAttack;
    public float                   attackRange;
    public int                     damage;
    public float                   attackForce;

    private SpriteRenderer         sprite;
    private Animator               animator;

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
        if (Input.GetKeyDown(KeyCode.Mouse0) && timeBtwAttack <= 0)
            Attack();
        else
            timeBtwAttack -= Time.deltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();
        if (Input.GetButton("Horizontal"))
            Run();
        
    }

    private void Run()
    {
        if(isGrounded)
            animator.SetInteger("State", 2);
        sprite.flipX = Input.GetAxis("Horizontal") < 0.0f;
        onRight = Input.GetAxis("Horizontal") > 0.0f;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
       //rb.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), 0);
        attackPos.transform.position = (onRight) ? new Vector2(transform.position.x + 0.75f, transform.position.y)
                                                 : new Vector2(transform.position.x - 0.75f, transform.position.y);
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f);
        isGrounded = (colliders.Length > 2) && !inCollWLadder;
    }

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            rb.gravityScale = 0;
            inCollWLadder = true;
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector2(0, verticalSpeed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.velocity = new Vector2(0, -verticalSpeed);
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        rb.gravityScale = gravityScale;
        inCollWLadder = false;
    }
}
