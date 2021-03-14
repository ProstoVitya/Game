using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image bar;
    public int maxHP;
    int currHP;
    private int fill;

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
        print(currHP);
    }

    public int GetHP() {
        return currHP;
    }
}
