using UnityEngine;

public class ObjectFollowing : MonoBehaviour {

    public float x, y, z;
    public GameObject player;

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(player.transform.position.x + x, player.transform.position.y + y, z);
    }
}
