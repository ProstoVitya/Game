using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{ 
    public enum Direction { 
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        NONE
    }

    public Direction direction;

    private RoomVariants variants;
    private int rand;
    private bool spawned = false;
    private readonly float waitTime = 3f;

    private void Start()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
        Destroy(gameObject, waitTime);
        Invoke(nameof(Spawn), 0.2f);
    }

    public void Spawn() {
        if (!spawned) {
            switch (direction) {
                case Direction.TOP:
                    rand = Random.Range(0, variants.topRooms.Length);
                    Instantiate(variants.topRooms[rand], transform.position, variants.topRooms[rand].transform.rotation);
                    break;
                case Direction.BOTTOM:
                    rand = Random.Range(0, variants.bottomRooms.Length);
                    Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
                    break;
                case Direction.LEFT:
                    rand = Random.Range(0, variants.leftRooms.Length);
                    Instantiate(variants.leftRooms[rand], transform.position, variants.leftRooms[rand].transform.rotation);
                    break;
                case Direction.RIGHT:
                    rand = Random.Range(0, variants.rightRooms.Length);
                    Instantiate(variants.rightRooms[rand], transform.position, variants.rightRooms[rand].transform.rotation);
                    break;
            }
            spawned = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomPoint") && collision.GetComponent<RoomSpawner>().spawned) {
            Destroy(gameObject);
        }
    }
}
