using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowCore : MonoBehaviour
{
    float rotationSpeed = 0.1f;
    int rotationSign;
    void Start()
    {
        rotationSign=Random.Range(0, 2);
        if(rotationSign ==0) { rotationSign = -1; }

        rotationSpeed=rotationSpeed*rotationSign;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed);
    }
}
