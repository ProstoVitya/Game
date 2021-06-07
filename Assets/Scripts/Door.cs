using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//скрипт функционала двери
public class Door : MonoBehaviour
{
    private Animator animator; //аниматор, включающий в себя анимации открытия/закрытия двери

    //запускается с началом работы скрипта
    //объявляет аниматор
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //метод открытия двери
    //переводит коллайдер объекта в состояние триггер(в этом состоянии объекты могу проходить сквозь него)
    //переключает аниматор в состояние открытой двери
    public void Open() {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        animator.SetInteger("State", 0);
    }

    //метод закрытия двери
    //переводит коллайдер в обычное состояние(в этом состоянии объекты не могу проходить сквозь него)
    //переключает аниматор в состояние закрытой двери
    public void Close() {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        animator.SetInteger("State", 1);
    }
}
