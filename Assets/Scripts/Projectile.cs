using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;//скорость снаряда
    public int avgDamage;//средний урон снаряда
    const int ENEMY_LAYER = 9;//слой с врагами

    //метод вызывается каждый фрейм
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);//двигаем объект вверх с заданной скоростью
    }
    //метод вызывается вначале контакта объектов
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)//если объект не тригер
        {
            if (collision.gameObject.name != "Boss" && collision.gameObject.layer != ENEMY_LAYER)
            {//если объект не босс и не враг
                if (collision.gameObject.CompareTag("Player")) //нанесение рандомного значения урона(средний урон +/- 3) игроку при попадании
                    collision.gameObject.GetComponent<HealthBar>().GetDamage(Random.Range(avgDamage - 3, avgDamage + 3));
                Destroy(gameObject);//уничтожаем объект
            }            
        }                
    }
    
}
