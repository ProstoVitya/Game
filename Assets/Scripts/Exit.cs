using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject wall;
    public Transform wallSpawner;
    private bool block = false;

   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Block") && !block) {
            Instantiate(wall, wallSpawner);
            block = true;
        }
        if (collision.CompareTag("Ladder") && block) {
            Destroy(collision.gameObject);
        }
        Destroy(wallSpawner.gameObject, 0.9f);
        Destroy(gameObject, 1f);
    }
}
