using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image bar;
    public int fill;
    private int currHP;
    
    void Start()
    {
        fill = gameObject.GetComponent<Player>().getHP();
        currHP = fill;
    }

    void Update()
    {
        currHP = gameObject.GetComponent<Player>().getHP();
        if(fill != currHP) { 
            bar.fillAmount = currHP * 0.01f;
            fill = currHP;
        }
    }
}
