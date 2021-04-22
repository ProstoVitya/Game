using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void Open() {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        animator.SetInteger("State", 0);
    }

    public void Close() {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        animator.SetInteger("State", 1);
    }
}
