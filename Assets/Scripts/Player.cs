using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int maxHP = 100;
    [SerializeField] private float jumpForce = 15f;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGrounded = false;
    private int currHP;
    public Main main;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        currHP = maxHP;
    }
    void FixedUpdate() 
    {
        CheckGround();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Horizontal"))
            Run();
        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();
    }
    private void Run()
    {
        /*rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        sprite.flipX = Input.GetAxis("Horizontal") < 0.0f;*/
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }
    private void Jump() 
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,0.3f);
        isGrounded = colliders.Length > 1;
    }

    public void GetDamage(int damage) 
    {
        currHP -= damage;
        print(currHP);
        if (currHP <= 0)
            Invoke("Lose", 1.5f);
    }
    void Lose() {
        main.GetComponent<Main>().Lose();
    }
}

