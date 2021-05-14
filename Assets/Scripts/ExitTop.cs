using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//выход вверх выделен отдельным скриптом
//существует для исправления бага с появлением комнаты, из которой нельзя выбраться
//отличается от Exit тем, что происходит удаление только длинной лестницы
//можно считать костылем, но очень красивым :)
public class ExitTop : MonoBehaviour
{
    public GameObject   wall;                 //объект стены
    public Transform    wallSpawner;          //точка спавна стены
    private bool        block        = false; //проверка ставился ли уже блок (костыль)
    public GameObject   door;                 //объект двери

    //метод работает в течение соприкосновения с объектом
    //убирает тупики при генерации комнат
    private void OnTriggerStay2D(Collider2D collision)
    {
        //при соприкосновении с блоком и если не ставился блок
        //ставится блок
        if (collision.CompareTag("Block") && !block)
        {
            Instantiate(wall, wallSpawner);
            block = true;
        }
        //при соприкосновении с длинной лестницей и поставленном блоке(лестница ведет в блок)
        if (collision.CompareTag("Ladder_l") && block)
        {
            Destroy(collision.gameObject); //уничтожаем лестницу
            Destroy(gameObject); //уничтожаем выход
        }

    }

    //метод работает при начале соприкосновения с обеъектом
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //соприкосновение с дверью и дверь еще не установлена
        if (collision.CompareTag("Door") && door == null)
            door = collision.gameObject; //привязывает дверь к выходу
    }
}
