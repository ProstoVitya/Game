using UnityEngine;

//скрипт работы шипов
public class Spike : MonoBehaviour
{
    //метод взывается вначале соприкосновения с объектом
    //наносит игроку 10 урона
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") //проверка соприкосновения с игроком
        {
            collision.gameObject.GetComponent<HealthBar>().GetDamage(10);
        }

    }
}
