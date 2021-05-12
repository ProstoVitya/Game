using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [Header("Exits")]
    public GameObject[] exits; //пустые объекты выходов

    [Header("Enemies")]
    public GameObject[] enemyTypes; //типы врагов которые могут появиться в комнате
    public Transform[] spawners; //места на карте, где могут появиться враги/бонусы

    [Header("Bonuses")]
    public GameObject[] bonusTypes; //типы бонусов которые могут появиться в комнате
    public Transform keySpawnPosition; //место где может появиться ключ

    public List<GameObject> enemies; //список врагов в одной комнате

    private RoomVariants variants; //массив вариантов комнат
    public bool spawned; //показывает спавнились ли враги в комнате

    //метод запускается еще до Start()
    //в ней задается массив комнат
    private void Awake()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
    }

    //запускается вначале работы скрипта
    //появившаяся комната добавляется в массив
    private void Start()
    {
        variants.rooms.Add(gameObject);
    }

    //метод активируется при соприкосновении коллайдеров
    //проверяется имеет ли соприкасающийся объект тэг "Player" и заходил ли этот объект в нее уже
    //если имеет и не заходил:
    //1 - на каждом спавнере появляется враг/бонус с вероятностью 9/1
    //2 - закрываются все двери, сосприкасающиеся с выходами из exits
    //3 - запускается корутн CheckEnemies()

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawned) {
            spawned = true;

            //1
            foreach (Transform spawner in spawners) {
                int rand = Random.Range(0, 11);
                if (rand < 10)
                {
                    GameObject emnemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                    GameObject enemy = Instantiate(emnemyType, spawner.position, Quaternion.identity);
                    enemy.transform.parent = transform;
                    enemies.Add(enemy);
                    Destroy(spawner.gameObject);
                }
                else {
                    GameObject bonusType = bonusTypes[Random.Range(0, bonusTypes.Length)];
                    Instantiate(bonusType, spawner.position, Quaternion.identity);
                    Destroy(spawner.gameObject);
                }
            }

            //2
            foreach (GameObject exit in exits) {
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
                
            //3
            StartCoroutine(CheckEnemies());
        }
    }

    //метод не дает коду продолжить работу пока:
    //1 - не пройдет 1 секунда(чтобы успели появиться враги, если они появятся)
    //2 - количество появившихся врагов в enemies != 0
    //при выполнении условий открывавются двери, соприкасающиеся с выходами в комнате
    IEnumerator CheckEnemies() {
        yield return new WaitForSeconds(1f); //1
        yield return new WaitUntil(() => enemies.Count == 0); //2
        //открываются двери
        foreach (GameObject exit in exits) {
            if (exit != null) {
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
    }
}
