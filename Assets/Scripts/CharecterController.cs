using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharecterController : MonoBehaviour {
    public float        horizontalSpeed;
    private float       speedX;
    public float        verticalImpulse;
    private Rigidbody2D rb;
    private bool        isGrounded;

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
        
        transform.Translate(speedX, 0, 0);
        speedX = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
    }
}
