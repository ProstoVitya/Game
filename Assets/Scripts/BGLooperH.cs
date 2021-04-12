using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLooperH : MonoBehaviour
{
    public float backgroundWidth;

    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10;
    private int leftIndex;
    private int rightIndex;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i) {
            layers[i] = transform.GetChild(i);
        }

        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }

    private void Update()
    {
        if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone + 5)) 
            ScrollLeft();
        
        else if (cameraTransform.position.x > (layers[leftIndex].transform.position.x - viewZone - 5))
            ScrollRight();

    }

    private void ScrollLeft() {
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundWidth);
        leftIndex = rightIndex;
        --rightIndex;

        if (rightIndex < 0)
            rightIndex = layers.Length - 1;
    }

    private void ScrollRight() {
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundWidth);
        rightIndex = leftIndex;
        ++leftIndex;

        if (leftIndex == layers.Length)
            leftIndex = 0;
    }
}
