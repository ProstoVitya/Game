using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [Header("Environment")]
    public GameObject door;//дверь

    [Header("Boss")]
    public GameObject bossHP;//хп босса
    public GameObject boss;//босс
    //метод вызывается в начале соприкосновения с объектом
    //при входе игрока в комнату закрываем дверь и включаем босса и его хп
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&boss!=null)
        {
            door.GetComponent<Door>().Close();
            bossHP.SetActive(true);
            boss.SetActive(true);
        }
        
    }
    //метод вызывается каждый фрейм
    //пока босс жив дверь остается закрытой, при смерти дверь открывается
    void Update() {
        if (boss == null)
            door.GetComponent<Door>().Open();
    }
}
