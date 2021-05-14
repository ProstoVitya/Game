using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//скрипт хранит массивы комнат
//отдельный массив на каждую сторону, в которую есть выход
//комнаты с несколькими выходами добавляются в несколько массивов
//ставит ключ к порталу в случайной комнате на точке его спавна
public class RoomVariants : MonoBehaviour
{
    public GameObject[]     topRooms;       //комнаты с выходом вверх
    public GameObject[]     bottomRooms;    //комнаты с выходом вниз
    public GameObject[]     leftRooms;      //комнаты с выходом влево
    public GameObject[]     rightRooms;     //комнаты с выходом вправо
    public GameObject       closedRoom;     //комната без выходов

    public GameObject       key;            //ключ к порталу

     public List<GameObject> rooms;         //список появившихся на уровне комнат

    //метод вызывается вначале работы скрипта
    //запускает корутин SpawnKey
    void Start()
    {
        StartCoroutine(SpawnKey());
    }

    //метод ожидает 3 секунды (чтобы дать появиться всем комнатам)
    //ставит ключ в случайной комнате, кроме комнаты без выходов
    private IEnumerator SpawnKey() {
        yield return new WaitForSeconds(3f);
        int rand = Random.Range(1, rooms.Count - 1); //генерируется случайное число
        while (rooms[rand].name == "ClosedRoom(Clone)")
            rand = Random.Range(1, rooms.Count - 1);
        //ставится ключ в комнате с индексом rand, на точке спавна, установленной в этой комнате
        Instantiate(key, rooms[rand].GetComponent<AddRoom>().keySpawnPosition.position, Quaternion.identity);
    }
}
