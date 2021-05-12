using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class EnemyPatrol : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Animator animator;
    private Transform player;
    public Transform patrolPoint;
    public Transform groundCheck;
    public LayerMask Ground;

    public float speed;
    public float patrolDistance;        
    public float stopDistance;
    public float cooldownAttack=0.6f;
    private float currentSpeed;
    private bool moveRight = true;
    private bool isAttacking = false;
    private bool patrol = false;
    private bool angry = false;
    private bool goBack = false;
    public bool isGround;
    private bool isWaiting = false;



    void MoveRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        moveRight = true;
    }
    void MoveLeft()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
        moveRight = false;
    }
    
        
   

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        
        animator = GetComponent<Animator>();
        currentSpeed = speed;
    }
    bool agrHeight()
    {
        return (-0.3f < (transform.position.y - player.position.y) && (transform.position.y - player.position.y) < 1.2f);
    }
    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f, Ground);
        isGround = colliders.Length > 0;
        if (cooldownAttack > 0)
            cooldownAttack -= Time.deltaTime;
        if (Mathf.Abs(transform.position.x - player.position.x) < patrolDistance && angry == false)
        {
            patrol = true;
        }
        if ( Mathf.Abs(transform.position.x-player.position.x) < stopDistance&& agrHeight()&& isGround)
        {
            angry = true;
            patrol = false;
            goBack = false;
        }
        if (Mathf.Abs(transform.position.x - player.position.x) > stopDistance || !agrHeight())
        {
            goBack = true;
            angry = false;
        }


        if (patrol == true && !isAttacking)
            Patrol();
        else if (angry == true && !isAttacking)
            Agr();
        else if (goBack == true && !isAttacking)
            GoBack();
    }
    void Patrol()
    {
        if (transform.position.x > patrolPoint.position.x + patrolDistance || (!isGround && moveRight))
            MoveLeft();
        else if (transform.position.x < patrolPoint.position.x - patrolDistance ||(!isGround && !moveRight))
            MoveRight();
        if (isWaiting)
        {
            isWaiting = false;
            animator.SetBool("isWaiting", false);
        }
        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
    }

    void Agr()
    {
        if (isGround)
        {
            if (player.position.x < transform.position.x && moveRight == true)
                MoveLeft();
            if (player.position.x > transform.position.x && moveRight == false)
                MoveRight();
            if (isWaiting)
            {
                isWaiting = false;
                animator.SetBool("isWaiting", false);
            }

            transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
        }
        else
        {
            isWaiting = true;
            animator.SetBool("isWaiting", true);           
        }
    }

    void GoBack()
    {
        if (patrolPoint.position.x < transform.position.x && moveRight == true)
            MoveLeft();
        if (patrolPoint.position.x > transform.position.x && moveRight == false)
            MoveRight();
        if (isWaiting)
        {
            isWaiting = false;
            animator.SetBool("isWaiting", false);
        }
        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
    }

    public void GetDamage(int damage)
    {
        currentSpeed = 0.40f;
        GetComponent<HealthBar>().GetDamage(damage);
        StartCoroutine(StopTime());
    }
    private IEnumerator StopTime()
    {
        yield return new WaitForSeconds(0.5f);
        currentSpeed = speed;
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (!isAttacking && cooldownAttack <= 0)
            {
                isAttacking = true;
                animator.SetFloat("animChance", Random.Range(0.0f, 1.0f));
                animator.SetBool("isAttacking", isAttacking);

                StartCoroutine(AttackTime());
            }
        }
        else if (!collision.isTrigger)
        {
            if (moveRight == true)
                MoveLeft();
            else
                MoveRight();
        }
    }
    private void onAttack()
    {
        player.gameObject.GetComponent<HealthBar>().GetDamage(Random.Range(6,11));
    }
    private IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(0.2f);
        cooldownAttack = 0.5f;
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
    }
}
