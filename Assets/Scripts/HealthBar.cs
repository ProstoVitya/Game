using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image      bar;
    public int        maxHP;
    int               currHP;
    private int       fill;
    public GameObject bloodEffect;

    void Start()
    {
        currHP = fill = maxHP;
    }

    void Update()
    {
        if (fill != currHP) {
            bar.fillAmount = currHP * 0.01f;
            fill = currHP;
        }
    }
    public void GetHeal(int healing) {
        currHP += healing;
        if (currHP > 100)
            currHP = maxHP;
    }
    public void GetDamage(int damage) {
        currHP -= damage;
        Destroy(Instantiate(bloodEffect, transform.position, Quaternion.identity), 1.5f);
    }

    public int GetHP() {
        return currHP;
    }
}
