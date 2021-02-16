using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharecterController : MonoBehaviour {
    public float        horizontalSpeed;
    public float        verticalSpeed;
    private float       speedX;
    private float       speedY;
    public float        verticalImpulse;
    private Rigidbody2D rb;
    private bool        isGrounded;
    private bool        onStairs;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.AddForce(new Vector2(0, verticalImpulse), ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate() {
        
        if (Input.GetKey(KeyCode.A)) {
            speedX = -horizontalSpeed;
        }
        else if (Input.GetKey(KeyCode.D)) {
            speedX = horizontalSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            speedY = verticalSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            speedY = -verticalSpeed;
        }

        if (onStairs)
            transform.Translate(0, speedY, 0);
        else
        {
            transform.Translate(speedX, 0, 0);
        }

        speedX = speedY = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;

        if (collision.gameObject.tag == "Stairs")
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;

        if (collision.gameObject.tag == "Stairs")
            isGrounded = false;
    }
}
