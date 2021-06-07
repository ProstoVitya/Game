using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    private float startPos1, startPos2, length, hight;//стартовые позиции области зрения камеры
    public GameObject camera;                         //камера
    public float paralaxEffect;                       //эффект паралакса
    
    //инициализируем переменные
    void Start()
    {
        startPos1 = transform.position.x;           
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        startPos2 = transform.position.y;
        hight = GetComponent<SpriteRenderer>().bounds.size.y;
    }
    
    void Update()
    {
        float temp = camera.transform.position.x * (1 - paralaxEffect);
        float dist = camera.transform.position.x * paralaxEffect;
        float temp1 = camera.transform.position.y * (1 - paralaxEffect);
        float dist2 = camera.transform.position.y * paralaxEffect;

        // двигаем фон с поправкой на paralaxEffect
        transform.position = new Vector3(startPos1 + dist, startPos2+dist2, 1);

        // если камера перескочила спрайт, то меняем startPos
        if (temp > startPos1 + length)
            startPos1 += length;
        else if (temp < startPos1 - length)
            startPos1 -= length;
        if (temp1 > startPos2 + hight)
            startPos2 += hight;
        else if (temp1 < startPos2 - hight)
            startPos2 -= hight;
    }
}
