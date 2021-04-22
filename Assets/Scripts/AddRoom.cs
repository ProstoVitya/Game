using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject[] enemyTypes;
    public Transform[] spawners;
    public Door[] doors;

    [Header("Bonuses")]
    public GameObject[] bonusTypes;

    [HideInInspector] public List<GameObject> enemies;

    private RoomVariants variants;
    private bool spawned;

    private void Start()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !spawned) {
            spawned = true;
            //закрываются двери
            foreach (Door door in doors)
                door.Close();

                foreach (Transform spawner in spawners) {
                int rand = Random.Range(0, 11);
                if (rand < 10)
                {
                    GameObject emnemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                    GameObject enemy = Instantiate(emnemyType, spawner.position, Quaternion.identity);
                }
                else {
                    //появляется бонус
                }
            }
            StartCoroutine(CheckEnemies());
        }
    }

    IEnumerator CheckEnemies() {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemies.Count == 0);
        //открываются двери
        foreach (Door door in doors)
            door.Open();
    }
}
