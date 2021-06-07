using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemyMini : MonoBehaviour
{
    public AudioClip hit; //звук удара
    public bool destroyed = false;//переменнная, чтоб при попытке удаления объекта мы не удаляли его несколько раз
    private Boss boss;//босс

    //запускается в начале работы скрипта
    //объявляет переменную boss
    void Start()
    {
        boss = GetComponentInParent<Boss>();
    }
    //активируется на каждый фрейм
    //проверяет количество жизни у объекта врага
    //если хп <= 0:
    //1 - уничтожение всех внутренних компонентов
    //2 - проигрывание звука
    //3 - удаляется родительский объект
    //4 - объект удаляется из списка врагов

    void Update()
    {
        if (!destroyed && gameObject.GetComponentInChildren<HealthBar>().GetHP() <= 0)
        {
            destroyed = true;
            foreach (Transform child in transform) Destroy(child.gameObject); //1
            GetComponent<AudioSource>().PlayOneShot(hit); //2
            Invoke("Destroy", 0.4f);//3
            boss.enemies.Remove(gameObject);//4
        }
    }

    //вспомогательный метод для уничтожения объекта
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
