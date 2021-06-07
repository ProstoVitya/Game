using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int avgDamage;
    const int ENEMY_LAYER = 9;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            if (collision.gameObject.name != "Boss" && collision.gameObject.layer != ENEMY_LAYER) {
                if (collision.gameObject.CompareTag("Player")) //нанесение рандомного значения урона(средний урон +/- 3) игроку при попадании
                    collision.gameObject.GetComponent<HealthBar>().GetDamage(Random.Range(avgDamage - 3, avgDamage + 3));
                Destroy(gameObject);
            }            
        }                
    }
    
}
