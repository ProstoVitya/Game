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

<<<<<<< HEAD
    private const int       MAX_ROOMS   = 10;

    public Direction        direction;
    private RoomVariants    variants;
    private static int      roomCount   = 0;
    private int             rand;
    private bool            spawned     = false;
    private float           waitTime    = 3f;
=======
    public Direction direction;
    private RoomVariants variants;
    private const int MAX_ROOMS = 10;
    private int rand;
    private bool spawned = false;
    private float waitTime = 3f;
>>>>>>> master

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
<<<<<<< HEAD
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.topRooms.Length - 1) : variants.topRooms.Length - 1;
=======
                    rand = Random.Range(0, variants.topRooms.Length);
>>>>>>> master
                    Instantiate(variants.topRooms[rand], transform.position, variants.topRooms[rand].transform.rotation);
                    break;

                case Direction.BOTTOM:
<<<<<<< HEAD
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.bottomRooms.Length - 1) : variants.bottomRooms.Length - 1;
                    Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
                    break;

                case Direction.LEFT:
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.leftRooms.Length - 1) : variants.leftRooms.Length - 1;
=======
                    rand = Random.Range(0, variants.bottomRooms.Length);
                    Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
                    break;

                case Direction.LEFT:                
                    rand = Random.Range(0, variants.leftRooms.Length);
>>>>>>> master
                    Instantiate(variants.leftRooms[rand], transform.position, variants.leftRooms[rand].transform.rotation);                 
                    break;

                case Direction.RIGHT:
<<<<<<< HEAD
                    rand = (roomCount < MAX_ROOMS) ? Random.Range(0, variants.rightRooms.Length - 1) : variants.rightRooms.Length - 1;
=======
                    rand = Random.Range(0, variants.rightRooms.Length);
>>>>>>> master
                    Instantiate(variants.rightRooms[rand], transform.position, variants.rightRooms[rand].transform.rotation);
                    break;
            }
            spawned = true;
<<<<<<< HEAD
            print(roomCount);
            ++roomCount;
=======
>>>>>>> master
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
<<<<<<< HEAD
        if (collision.CompareTag("RoomPoint")) {
            if(!collision.GetComponent<RoomSpawner>().spawned && !spawned) {
                Instantiate(variants.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
=======
        if (collision.CompareTag("RoomPoint") && collision.GetComponent<RoomSpawner>().spawned) {
            Destroy(gameObject);
>>>>>>> master
        }
    }
}
