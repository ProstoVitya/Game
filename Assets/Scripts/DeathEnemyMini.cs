using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemyMini : MonoBehaviour
{
    //public GameObject[] bonusTypes; //бонусы которые могут выпасть при убийстве
    //public GameObject effectDrop; //эффект выпадения бонуса
    public AudioClip hit; //звук удара
    public bool destroyed = false;//переменнная, чтоб при попытке удаления объекта мы не удаляли его несколько раз
    private Boss boss;

    //запускается в начале работы скрипта
    //объявляет переменную boss
    void Start()
    {
        boss = GetComponentInParent<Boss>();
    }

    
    void Update()
    {
        if (!destroyed && gameObject.GetComponentInChildren<HealthBar>().GetHP() <= 0)
        {
            destroyed = true;
            foreach (Transform child in transform) Destroy(child.gameObject); //1
            GetComponent<AudioSource>().PlayOneShot(hit); //2
            /*if (Random.Range(0f, 1f) < 0.08f) //3
            {
                GameObject bonusType = bonusTypes[Random.Range(0, bonusTypes.Length)];
                Instantiate(effectDrop, GetComponentInChildren<EnemyPatrol>().GetComponent<Transform>().position, Quaternion.identity);
                Instantiate(bonusType, GetComponentInChildren<EnemyPatrol>().GetComponent<Transform>().position, Quaternion.identity);
            }*/
            Invoke("Destroy", 0.4f);//4
            boss.enemies.Remove(gameObject);//5
        }
    }

    //вспомогательный метод для уничтожения объекта
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
