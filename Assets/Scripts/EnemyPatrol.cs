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

    public float speed;
    public float patrolDistance;        
    public float stopDistance;
    private float cooldownAttack=0.6f;
    private float currentSpeed;
    private bool moveRight = true;
    private bool isAttacking = false;
    public bool patrol = false;
    public bool angry = false;
    public bool goBack = false;

    

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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        
        animator = GetComponent<Animator>();
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownAttack > 0)
            cooldownAttack -= Time.deltaTime;
        if (Vector2.Distance(transform.position, patrolPoint.position) < patrolDistance && angry == false)
        {
            patrol = true;
        }
        if (Vector2.Distance(transform.position, player.position) < stopDistance)
        {
            angry = true;
            patrol = false;
            goBack = false;
        }
        if (Vector2.Distance(transform.position, player.position) > stopDistance)
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
        if (transform.position.x > patrolPoint.position.x + patrolDistance)
            MoveLeft();
        if (transform.position.x < patrolPoint.position.x - patrolDistance)
            MoveRight();

        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
    }

    void Agr()
    {
        if (player.position.x < transform.position.x && moveRight == true)
            MoveLeft();
        if (player.position.x > transform.position.x && moveRight == false)
            MoveRight();
        transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
    }

    void GoBack()
    {
        if (patrolPoint.position.x < transform.position.x && moveRight == true)
            MoveLeft();
        if (patrolPoint.position.x > transform.position.x && moveRight == false)
            MoveRight();
        transform.position = Vector2.MoveTowards(transform.position, patrolPoint.position, currentSpeed * Time.deltaTime);
    }

    public void GetDamage(int damage)
    {
        currentSpeed = 0.6f;
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
        player.gameObject.GetComponent<HealthBar>().GetDamage(Random.Range(7,12));
    }
    private IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(0.2f);
        cooldownAttack = 0.6f;
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
    }
}
