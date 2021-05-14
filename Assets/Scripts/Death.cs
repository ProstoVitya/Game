using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
//скрипт смерти врага
public class Death : MonoBehaviour
{
    private AddRoom     room; //комната в которой появляется враг (каждый привязывается к своей комнате)
    public GameObject[] bonusTypes; //бонусы которые могут выпасть при убийстве
    public GameObject   effectDrop; //эффект выпадения бонуса

    //запускается в начале работы скрипта
    //объявляет переменную room
    void Start() {
        room = GetComponentInParent<AddRoom>();
    }

    //активируется на каждый фрейм
    //проверяет количество жизни у объекта врага
    //если хп <= 0:
    //1 - уничтожает объект
    //2 - с вероятностью 8% на месте объекта появляется бонус
    //3 - объект удаляется из списка врагов
    void Update()
    {
        if (gameObject.GetComponentInChildren<HealthBar>().GetHP() <= 0) {            
            Destroy(gameObject); //1
            if (Random.Range(0f, 1f) < 0.08f) //2
            {
                GameObject bonusType = bonusTypes[Random.Range(0, bonusTypes.Length)];
                Instantiate(effectDrop, GetComponentInChildren<EnemyPatrol>().GetComponent<Transform>().position, Quaternion.identity);
                Instantiate(bonusType,GetComponentInChildren<EnemyPatrol>().GetComponent<Transform>().position, Quaternion.identity);
            }
            room.enemies.Remove(gameObject);//3
        }
           
    }
}
