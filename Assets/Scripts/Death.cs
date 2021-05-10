using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private AddRoom room;

    public GameObject[] bonusTypes;
    public GameObject effectDrop;
    void Start() {
        room = GetComponentInParent<AddRoom>();
    }
    void Update()
    {
        if (gameObject.GetComponentInChildren<HealthBar>().GetHP() <= 0) {            
            Destroy(gameObject);
            if (Random.Range(0f, 1f) < 0.08f)
            {
                GameObject bonusType = bonusTypes[Random.Range(0, bonusTypes.Length)];
                Instantiate(effectDrop, GetComponentInChildren<EnemyPatrol>().GetComponent<Transform>().position, Quaternion.identity);
                Instantiate(bonusType,GetComponentInChildren<EnemyPatrol>().GetComponent<Transform>().position, Quaternion.identity);
            }
            room.enemies.Remove(gameObject);
        }
           
    }
}
