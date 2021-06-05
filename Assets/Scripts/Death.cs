using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
//скрипт смерти врага
public class Death : MonoBehaviour
{
    private AddRoom     room; //комната в которой появляется враг (каждый привязывается к своей комнате)
    public GameObject[] bonusTypes; //бонусы которые могут выпасть при убийстве
    public GameObject   effectDrop; //эффект выпадения бонуса
    public AudioClip    hit; //звук удара
    public bool         destroyed = false;

    //запускается в начале работы скрипта
    //объявляет переменную room
    void Start() {
        room = GetComponentInParent<AddRoom>();
    }

    //активируется на каждый фрейм
    //проверяет количество жизни у объекта врага
    //если хп <= 0:
    //1 - уничтожение всех внутренних компонентов
    //2 - проигрывание звука
    //3 - с вероятностью 8% на месте объекта появляется бонус
    //4 - удаляется родительский объект
    //5 - объект удаляется из списка врагов
    void Update()
    {
        if (!destroyed && gameObject.GetComponentInChildren<HealthBar>().GetHP() <= 0)
        {
            destroyed = true;
            foreach (Transform child in transform) Destroy(child.gameObject); //1
            GetComponent<AudioSource>().PlayOneShot(hit); //2
            if (Random.Range(0f, 1f) < 0.08f) //3
            {
                GameObject bonusType = bonusTypes[Random.Range(0, bonusTypes.Length)];
                Instantiate(effectDrop, GetComponentInChildren<EnemyPatrol>().GetComponent<Transform>().position, Quaternion.identity);
                Instantiate(bonusType,GetComponentInChildren<EnemyPatrol>().GetComponent<Transform>().position, Quaternion.identity);
            }
            Invoke("Destroy", 0.4f);//4
            room.enemies.Remove(gameObject);//5
        }
    }

    //вспомогательный метод для уничтожения родительского объекта
    private void Destroy() {
        Destroy(gameObject);
    }
}
