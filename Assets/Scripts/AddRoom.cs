using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//скрипт функционала комнат
public class AddRoom : MonoBehaviour
{
    [Header("Exits")]
    public GameObject[]         exits;            //массив выходов из комнаты

    [Header("Enemies")]
    public GameObject[]         enemyTypes;       //типы врагов, которые могут появиться в комнате
    public Transform[]          spawners;         //места появления врагов/бонусов

    [Header("Bonuses")]
    public GameObject[]         bonusTypes;       //типы бонусов, которые могут появиться в комнате
    public Transform            keySpawnPosition; //место в котором может появиться ключ

    public List<GameObject>     enemies;          //список враго, появившихся в комнате

    private RoomVariants        variants;         //варианты комнат
    public bool                 spawned;          //проверка, заходил ли в комнату игрок

    [Header("Bonuses")]
    private AudioSource roomFX;
    public AudioClip openDoor;
    public AudioClip closeDoor;

    //метод вызывается перед Start()
    //объявление переменной вариантов комнат
    private void Awake()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
    }

    //метод вызывается в начале работы скрипта(при создании комнаты)
    //комната добавляется в список комнат на уровне
    private void Start()
    {
        variants.rooms.Add(gameObject);
        roomFX = GetComponent<AudioSource>();
    }

    //метод вызывается в начале соприкосновения с объектом
    //при входе игрока в комнату и он еще не заходил в нее:
    //1 - на каждом спавнере с вероятностью 9/1 появляется враг/бонус
    //2 - закрываются все входы в комнату
    //3 - запускается корутин, проверяющих наличие врагов в комнате
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawned)
        {
            spawned = true;

            foreach (Transform spawner in spawners) //1
            {
                int rand = Random.Range(0, 11);
                if (rand < 10)
                {
                    GameObject emnemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                    GameObject enemy = Instantiate(emnemyType, spawner.position, Quaternion.identity);
                    enemy.transform.parent = transform;
                    enemies.Add(enemy);
                    Destroy(spawner.gameObject);
                }
                else
                {
                    GameObject bonusType = bonusTypes[Random.Range(0, bonusTypes.Length)];
                    Instantiate(bonusType, spawner.position, Quaternion.identity);
                    Destroy(spawner.gameObject);
                }
            }

            foreach (GameObject exit in exits) //2
            {
                if (exit != null)
                {
                    if (exit.TryGetComponent(out Exit exit0))
                    {
                        if (exit0.door != null)
                            exit0.door.GetComponent<Door>().Close();
                    }
                    else if (exit.TryGetComponent(out ExitTop exittop))
                    {
                        if (exittop.door != null)
                            exittop.door.GetComponent<Door>().Close();
                    }
                }
            }
            if (roomFX != null)
                roomFX.PlayOneShot(closeDoor);
            StartCoroutine(CheckEnemies()); //3
        }
    }

    //метод останавливает выполнение кода, пока не выполнятся условия:
    //не пройдет одна секунда(сделано, чтобы на точках спавна успели появиться враги)
    //количество элементов в списке врагов !=0
    //после этого открываются все выходы из комнат
    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemies.Count == 0);
        //открываются двери
        foreach (GameObject exit in exits)
        {
            if (exit != null)
            {
                if (exit.TryGetComponent(out Exit exit0))
                {
                    if (exit0.door != null)
                        exit0.door.GetComponent<Door>().Open();
                }
                else if (exit.TryGetComponent(out ExitTop exittop))
                {
                    if (exittop.door != null)
                        exittop.door.GetComponent<Door>().Open();
                }
            }
        }
        if(roomFX != null)
            roomFX.PlayOneShot(openDoor);
    }
}
