using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 3f;
    public float patrolDistance;
    public Transform patrolPoint;
    private bool moveRight = true;
    private Vector2 dir = Vector2.right;
    private Transform player;
    public float stopDistance;
    private SpriteRenderer sprite;
    public Transform wallDetect;
    bool patrol = false;
    bool angry = false;
    bool goBack = false;

    private AddRoom room;

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
        room = GetComponentInParent<AddRoom>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthBar>().GetDamage(10);
        }

    }


    // Update is called once per frame
    void Update()
    {
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


        if (patrol == true)
            Patrol();
        else if (angry == true)
            Agr();
        else if (goBack == true)
            GoBack();

        if (gameObject.GetComponent<HealthBar>().GetHP() <= 0)
            room.enemies.Remove(gameObject);
    }
    void Patrol()
    {
        RaycastHit2D gr = Physics2D.Raycast(wallDetect.position, Vector2.right, 0.05f);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallDetect.position, 0.05f);
        if (colliders.Length > 1 && colliders.All(x => !x.GetComponent<PlayerController>()))//разворот
        {
            if (moveRight == true)
                MoveLeft();
            else
                MoveRight();
        }
        if (transform.position.x > patrolPoint.position.x + patrolDistance)
            MoveLeft();
        if (transform.position.x < patrolPoint.position.x - patrolDistance)
            MoveRight();

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void Agr()
    {
        if (player.position.x < transform.position.x && moveRight == true)
            MoveLeft();
        if (player.position.x > transform.position.x && moveRight == false)
            MoveRight();
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void GoBack()
    {
        if (patrolPoint.position.x < transform.position.x && moveRight == true)
            MoveLeft();
        if (patrolPoint.position.x > transform.position.x && moveRight == false)
            MoveRight();
        transform.position = Vector2.MoveTowards(transform.position, patrolPoint.position, speed * Time.deltaTime);
    }

    public void PushAway(Vector3 pushFrom, float pushPower)
    {
        if (GetComponent<Rigidbody2D>() == null)
            return;
        Vector3 pushDirection = pushFrom - transform.position;
        GetComponent<Rigidbody2D>().AddForce(pushDirection * pushPower);
    }
}