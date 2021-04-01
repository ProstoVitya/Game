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

    private const int       MAX_ROOMS   = 10;

    public Direction        direction;
    private RoomVariants    variants;
    private static int      roomCount   = 0;
    private int             rand;
    private bool            spawned     = false;
    private float           waitTime    = 3f;

    private void Start()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
        Destroy(gameObject, waitTime);
        Invoke("Spawn", 0.2f);
    }

    public void Spawn() {
        if (!spawned) {
            switch (direction) {
                case Direction.TOP:
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.topRooms.Length - 1) : variants.topRooms.Length - 1;
                    Instantiate(variants.topRooms[rand], transform.position, variants.topRooms[rand].transform.rotation);
                    break;

                case Direction.BOTTOM:
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.bottomRooms.Length - 1) : variants.bottomRooms.Length - 1;
                    Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
                    break;

                case Direction.LEFT:
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.leftRooms.Length - 1) : variants.leftRooms.Length - 1;
                    Instantiate(variants.leftRooms[rand], transform.position, variants.leftRooms[rand].transform.rotation);                 
                    break;

                case Direction.RIGHT:
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.rightRooms.Length - 1) : variants.rightRooms.Length - 1;
                    Instantiate(variants.rightRooms[rand], transform.position, variants.rightRooms[rand].transform.rotation);
                    break;
            }
            spawned = true;
            print(roomCount);
            ++roomCount;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomPoint")) {
            if(!collision.GetComponent<RoomSpawner>().spawned && !spawned) {
                Instantiate(variants.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
