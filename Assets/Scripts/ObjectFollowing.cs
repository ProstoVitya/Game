using UnityEngine;

//скрипт для прикрепления объекта к другому
public class ObjectFollowing : MonoBehaviour {

    public float        x, y, z;    //на каком расстоянии от player  должен денржаться объект
    public GameObject   player;     //объект к которому нужно прикрепить

    //метод вызывается на каждый фрейм
    //изменяет координаты прикрепленного объекта
    void Update() {
        transform.position = new Vector3(player.transform.position.x + x, player.transform.position.y + y, z);
    }
}
