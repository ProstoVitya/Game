using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject wall;
    public Transform wallSpawner;
    private bool block = false;
    public GameObject door;

   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Block") && !block) {
            Instantiate(wall, wallSpawner);
            block = true;
        }
        if (collision.CompareTag("Ladder") && block) {
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Door") && block)
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
