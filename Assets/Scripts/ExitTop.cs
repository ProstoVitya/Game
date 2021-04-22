using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTop : MonoBehaviour
{
    public GameObject wall;
    public Transform wallSpawner;
    private bool block = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Block") && !block)
        {
            Instantiate(wall, wallSpawner);
            block = true;
        }
        if ((collision.CompareTag("Ladder") || collision.CompareTag("Ladder_l")) && block)
        {
            Destroy(collision.gameObject);
        }
   
        //Destroy(wallSpawner.gameObject, 0.9f);
        Destroy(gameObject, 1f);
    }
}
