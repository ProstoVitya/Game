﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]

    float speed = 5;
    private float gravityScale = 5;

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().gravityScale = 0;
        if(collision.gameObject.CompareTag("Player"))
        {
            if(Input.GetKey(KeyCode.W))
            {
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
            }
            else {
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            collision.GetComponent<PlayerController>().is_climbimg = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
        collision.GetComponent<PlayerController>().is_climbimg = false;
    }
}
