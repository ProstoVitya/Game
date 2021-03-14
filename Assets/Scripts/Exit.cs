using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject, 0.6f);
    }
}
