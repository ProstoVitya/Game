using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public GameObject player;

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -12.29758f);
    }
}
