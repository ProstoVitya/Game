using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLooperV : MonoBehaviour
{
    public float backgroundWidth;

    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 5;
    private int upIndex;
    private int downIndex;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i)
        {
            layers[i] = transform.GetChild(i);
        }

        upIndex = 0;
        downIndex = layers.Length - 1;
    }

    private void Update()
    {
        if (cameraTransform.position.y < (layers[upIndex].transform.position.y + viewZone +1))
            ScrollUp();

        else if (cameraTransform.position.y > (layers[upIndex].transform.position.y - viewZone - 1))
            ScrollDown();

    }

    private void ScrollUp()
    {
        layers[downIndex].position = Vector3.up * (layers[upIndex].position.y - backgroundWidth);
        upIndex = downIndex;
        --downIndex;

        if (downIndex < 0)
            downIndex = layers.Length - 1;
    }

    private void ScrollDown()
    {
        layers[upIndex].position = Vector3.up * (layers[downIndex].position.y + backgroundWidth);
        downIndex = upIndex;
        ++upIndex;

        if (upIndex == layers.Length)
            upIndex = 0;
    }
}
