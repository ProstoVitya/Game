using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    void Update()
    {
        if (gameObject.GetComponentInChildren<HealthBar>().GetHP() <= 0)
            Destroy(gameObject);
    }
}
