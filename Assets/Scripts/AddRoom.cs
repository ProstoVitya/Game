using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [Header("Exits")]
    public GameObject[] exits;

    [Header("Enemies")]
    public GameObject[] enemyTypes;
    public Transform[] spawners;

    [Header("Bonuses")]
    public GameObject[] bonusTypes;

    public List<GameObject> enemies;

    private RoomVariants variants;
    public bool spawned;

    private void Start()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawned) {
            spawned = true;

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
                    //появляется бонус
                }
            }
            //закрываются двери
            foreach (GameObject exit in exits) {
                if (exit != null)
                {
                    if (exit.TryGetComponent(out Exit exit0))
                        exit0.door.GetComponent<Door>().Close();
                    else if (exit.TryGetComponent(out ExitTop exittop))
                        exittop.door.GetComponent<Door>().Close();
                }
                
            }
                
            StartCoroutine(CheckEnemies());
        }
    }
    
    IEnumerator CheckEnemies() {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemies.Count == 0);
        //открываются двери
        foreach (GameObject exit in exits) {
            if (exit != null)
                if (exit.TryGetComponent(out Exit exit0))
                    exit0.door.GetComponent<Door>().Open();
                else if (exit.TryGetComponent(out ExitTop exittop))
                    exittop.door.GetComponent<Door>().Open();
        }
            
    }
}
