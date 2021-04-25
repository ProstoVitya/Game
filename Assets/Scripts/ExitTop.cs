using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTop : MonoBehaviour
{
    public GameObject wall;
    public Transform wallSpawner;
    private bool block = false;
    public GameObject door;
    public GameObject ladder;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Block") && !block)
        {
            Instantiate(wall, wallSpawner);
            block = true;
        }
        if (collision.CompareTag("Ladder_l") && block)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door") && door == null)
            door = collision.gameObject;
    }
}
