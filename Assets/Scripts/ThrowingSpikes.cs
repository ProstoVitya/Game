using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingSpikes : MonoBehaviour
{
    public Transform SpawnPoint;//точка спавна шипов
    public GameObject spike;    //шип
    public float reload;        //время перезарядки
    public bool waves;          //проверка есть ли волны
    public int ProjectilesCountInWave;  //количество выстрелов в волне
    public float reloadwaves;           //время перезарядки волн
    //метод вызывается в начале работы скрипта
    void Start()
    {
        if (waves)//если есть волны запускаем корутину генерации шипов с волнами
            StartCoroutine(generateObjWithWaves());
        else
            StartCoroutine(generateObj());
    }
    //корутина генерации шипов без волн
    private IEnumerator generateObj()
    {
        while (true) {           
            Instantiate(spike, SpawnPoint.position, SpawnPoint.rotation);//генерируем объект
            yield return new WaitForSeconds(reload);//ждем время перезарядки
        }
    }
    //корутина генерации шипов волнами
    private IEnumerator generateObjWithWaves()
    {
        while (true)
        {
            for (int i = 0; i < ProjectilesCountInWave; ++i) {                
                Instantiate(spike, SpawnPoint.position, SpawnPoint.rotation);//генерируем объект
                yield return new WaitForSeconds(reload);//ждем время перезарядки
            }
            yield return new WaitForSeconds(reloadwaves);//ждем время перезарядки между волнами
        }
    }

}
