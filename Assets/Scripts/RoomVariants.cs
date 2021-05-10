using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVariants : MonoBehaviour
{
    public GameObject[]     topRooms;
    public GameObject[]     bottomRooms;
    public GameObject[]     leftRooms;
    public GameObject[]     rightRooms;
    public GameObject       closedRoom;

    public GameObject       key;

     public List<GameObject> rooms;
    void Start()
    {
        StartCoroutine(SpawnKey());
    }
    private IEnumerator SpawnKey() {
        yield return new WaitForSeconds(3f);
        int rand = Random.Range(1, rooms.Count - 1);
        while (rooms[rand].name == "ClosedRoom(Clone)")
            rand = Random.Range(1, rooms.Count - 1);
        Instantiate(key, rooms[rand].GetComponent<AddRoom>().keySpawnPosition.position, Quaternion.identity);
    }
}
