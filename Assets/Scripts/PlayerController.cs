using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private bool facingRight = true;

    private Rigidbody2D rb;

    public int jumpsValue;
    private int extraJumps;
    private bool isGrounded;

    // Start is called before the first frame update
    private void Start()
    {
        extraJumps = jumpsValue;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if(!facingRight && moveInput > 0)
        {
            Flip();

        } else if(facingRight && moveInput < 0)
        {
            Flip();
        }
    }

    private void Update()
    {
        if (isGrounded) {
            extraJumps = jumpsValue;
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            --extraJumps;
        }
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

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
}
