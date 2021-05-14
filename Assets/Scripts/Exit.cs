using UnityEngine;
using System.Collections;

//скрипт пустого объекта выхода
public class Exit : MonoBehaviour
{
    public GameObject    wall;                 //объект стены
    public Transform     wallSpawner;          //точка спавна стены
    private bool         block        = false; //костыль
    public GameObject    door;                 //объект двери

    //метод работает в течение соприкосновения с объектом
    //убирает тупики при генерации комнат
    private void OnTriggerStay2D(Collider2D collision)
    {
        //если выход соприкасается с блоком (в этом месте тупик) и стена еще не ставилась
        if (collision.CompareTag("Block") && !block) {
            Instantiate(wall, wallSpawner); //создается стена на месте точки wallSpawner
            block = true;
        }
        //если соприкосновение с лестницей и поставлен блок (значит лестница ведет в стену)
        if (collision.CompareTag("Ladder") && block) {
            Destroy(collision.gameObject); //уничтожаем лестницу
        }
        //если соприкосновение с дверью и поставлена стена (дверь находится в стене)
        if (collision.CompareTag("Door") && block)
        {
            Destroy(collision.gameObject); //удаление двери
            Destroy(gameObject); //удаление выхода
        }
    }

    //метод работает при начале соприкосновения с обеъектом
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //если соприкосновение с дверью и дверь еще не объявлена
        if (collision.CompareTag("Door") && door == null)
            door = collision.gameObject; //к выходу привязывается дверь
    }
}
