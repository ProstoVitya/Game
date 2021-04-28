using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private AddRoom room;
    void Start() {
        room = GetComponentInParent<AddRoom>();
    }
    void Update()
    {
        if (gameObject.GetComponentInChildren<HealthBar>().GetHP() <= 0) {            
            Destroy(gameObject);
            room.enemies.Remove(gameObject);
        }
           
    }
}
