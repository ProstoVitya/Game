using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image      bar;         //изображение полоски хп
    public int        maxHP;       //максимальное количество хп
    int               currHP;      //текущее количество хп
    private int       fill;        //переменная заполнения bar
    public GameObject bloodEffect; //эффект крови

    //метод запускается в начале работы скрипта
    //приравнивает все к максимальному хп
    void Start()
    {
        currHP = fill = maxHP;
    }

    //метод вызывается на каждый фрейм
    //заполняет bar в зависимости от текущего количества хп
    void Update()
    {
        if (fill != currHP) { //если заполнение != текущему хп
            bar.fillAmount = currHP * 0.01f; //перезаполнение полоски bar
            fill = currHP;
        }
    }

    //метод лечения
    //принимает количество хп которое нужно восстановить
    public void GetHeal(int healing) {
        currHP += healing;
        if (currHP > 100)
            currHP = maxHP;
    }

    //метод получения урона
    //принимает количество хп которое нужно снять
    //создает эффект крови и уничтожает его через 1.5 секунды
    public void GetDamage(int damage) {
        currHP -= damage;
        Destroy(Instantiate(bloodEffect, transform.position, Quaternion.identity), 1.5f);
    }

    //геттер хп
    public int GetHP() {
        return currHP;
    }
}
