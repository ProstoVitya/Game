using UnityEngine;

//метод слчайной расстановки комнат на уровне
//ставится на спавнеры у выходов из комнат
public class RoomSpawner : MonoBehaviour
{
    //перечисление направлений в которых у комнаты есть выходы
    public enum Direction { 
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        NONE
    }

    private const int       MAX_ROOMS   = 10;       //максимальное количество комнат на уровне

    public Direction        direction;              //направление
    private RoomVariants    variants;               //варианты комнат
    private static int      roomCount   = 0;        //счетчик поставленных комнат
    private int             rand;                   //переменная для сохранения случайного значения
    private bool            spawned     = false;    //проверка, поставлена ли комната на точке спавна
    private float           waitTime    = 3f;       //время ожидания уничтожения спавнеров комнат


    //метод вызывается с началом работы скрипта
    //объявляется переменная вариантов комнат
    //через 3 секунды уничтожаются все спавнеры
    //вызывает метод Spawn через 0.2 секунды после старта
    private void Start()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
        Destroy(gameObject, waitTime);
        Invoke("Spawn", 0.2f);
    }

    //метод ставит новую комнату с нужным выходом
    //выводит в консоль количество комнат (для отладки)
    //увеличивает счетчик появившихся комнат
    //1 - проверяем комнату с выходом в какую сторону нужно поставить на текущее место
    //2 - если количество комнат меньше максимального желаемого, генерируется случайное число
    //    в диапазоне [0, variants.<Направление>.Length - 2),
    //    иначе [variants.<Направление>.Length - 3, variants.<Направление>.Length)
    //так как последние 3 комнаты в каждом массиве "тупиковые" (то есть только с одним выходом)
    //3 - создаем комнату с номером rand из массива комнат с нужным направлением в точке спавна
    public void Spawn() {
        if (!spawned) { //если комната еще не появилась
            switch (direction) //1
            {
                case Direction.TOP:
                    //2
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.topRooms.Length - 2) :
                                                     Random.Range(variants.topRooms.Length - 3, variants.topRooms.Length);
                    //3
                    Instantiate(variants.topRooms[rand], transform.position, variants.topRooms[rand].transform.rotation);
                    break;

                case Direction.BOTTOM:
                    //2
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.bottomRooms.Length - 2) :
                                                     Random.Range(variants.bottomRooms.Length - 3, variants.bottomRooms.Length);
                    //3
                    Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
                    break;

                case Direction.LEFT:
                    //2
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.leftRooms.Length - 2) :
                                                     Random.Range(variants.leftRooms.Length - 3, variants.leftRooms.Length);
                    //3
                    Instantiate(variants.leftRooms[rand], transform.position, variants.leftRooms[rand].transform.rotation);                 
                    break;

                case Direction.RIGHT:
                    //2
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.rightRooms.Length - 2) :
                                                     Random.Range(variants.rightRooms.Length - 3, variants.rightRooms.Length);
                    //3
                    Instantiate(variants.rightRooms[rand], transform.position, variants.rightRooms[rand].transform.rotation);
                    break;
            }
            spawned = true;
            print(roomCount);
            ++roomCount;
        }
    }

    //метод работает в течение соприкосновения с объектом
    //ставит комнату без выходов в случае соприкосновения точек спавна
    //создано во избежние наслаивания комнат друг на друга и бесконечной генерации
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomPoint")) {
            if(!collision.GetComponent<RoomSpawner>().spawned && !spawned) {
                Instantiate(variants.closedRoom, transform.position, Quaternion.identity); //создание тупиковой комнаты
                Destroy(gameObject); //уничтожение точек спавна
            }
            spawned = true;
        }
    }
}
