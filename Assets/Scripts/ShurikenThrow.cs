using UnityEngine;

public class ShurikenThrow : MonoBehaviour
{
    const int ENEMY_LAYER = 9;//слой с врагами
    public float speed; //скорость
    public int  damage; //урон

    //Метод измененяет положение объекта
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    //метод соприкосновения с объектом
    //при соприкосновении с врагом наносит урон и уничтожается
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger&& !collision.gameObject.CompareTag("Player")) {
            if (collision.gameObject.layer == ENEMY_LAYER)
            {
                if (collision.TryGetComponent(out EnemyPatrol enemy))
                    enemy.GetComponent<EnemyPatrol>().GetDamage(damage);
                else if (collision.TryGetComponent(out Boss boss))
                    boss.GetComponent<Boss>().GetDamage(damage);
            }
            Destroy(gameObject);
        }       
    }
}
