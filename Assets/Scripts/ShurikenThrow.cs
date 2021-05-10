using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenThrow : MonoBehaviour
{
    public float speed;
    public int damage;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger) {
            if(collision.gameObject.layer == 9)
                collision.GetComponent<EnemyPatrol>().GetDamage(damage);
            Destroy(gameObject);
        }       
    }
}
