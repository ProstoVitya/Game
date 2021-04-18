using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
<<<<<<< HEAD
    public Image      bar;
    public int        maxHP;
    int               currHP;
    private int       fill;
    public GameObject bloodEffect;
=======
    public Image bar;
    public int maxHP;
    int currHP;
    private int fill;
>>>>>>> master

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

    public void GetDamage(int damage) {
        currHP -= damage;
<<<<<<< HEAD
        Destroy(Instantiate(bloodEffect, transform.position, Quaternion.identity), 1.5f);
=======
        print(currHP);
>>>>>>> master
    }

    public int GetHP() {
        return currHP;
    }
}
