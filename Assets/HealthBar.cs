using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image bar;
    public float fill;
    // Start is called before the first frame update
    void Start()
    {
        fill = 1f;
    }
    void Damage(int d,int MaxHealth)
    {
        fill -= d / MaxHealth;
    }
    void Heal(int h, int MaxHealth)
    {
        fill += h / MaxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = fill;
    }
}
