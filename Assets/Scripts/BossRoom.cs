using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [Header("Environment")]
    public GameObject door;

    [Header("Boss")]
    public GameObject bossHP;
    public GameObject boss;
    //метод вызывается в начале соприкосновения с объектом
    //при входе игрока в комнату
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&boss!=null)
        {
            door.GetComponent<Door>().Close();
            bossHP.SetActive(true);
            boss.SetActive(true);
        }
        
    }
    void Update() {
        if (boss == null)
            door.GetComponent<Door>().Open();
    }
}
