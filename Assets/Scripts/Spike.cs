using UnityEngine;
using System.Collections;
//скрипт работы шипов
public class Spike : MonoBehaviour
{
    public int damage;//урон
    private bool canDamage=true;//проверка может ли нанести урон
    //метод взывается вначале соприкосновения с объектом
    //наносит игроку 10 урона
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player"&&canDamage) //проверка соприкосновения с игроком
        {
            //наносим урон и ставим кулдаун на урон
            collision.gameObject.GetComponent<HealthBar>().GetDamage(damage);
            canDamage = false;
            StartCoroutine(CooldownDamage());//запускаем корутину 
        }

    }
    //корутина на откат кулдауна
    private IEnumerator CooldownDamage()
    {
        yield return new WaitForSeconds(0.5f);
        canDamage = true;
    }
}
