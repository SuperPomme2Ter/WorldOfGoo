using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorExample : MonoBehaviour
{
    [SerializeField] private ColliderDetector ColliderA;
    [SerializeField] private ColliderDetector ColliderB;
    void Start()
    {
        ColliderA.onTriggerEnterFunction= x => { Debug.Log("Blue"); } ;
        ColliderB.onTriggerEnterFunction= x => { Debug.Log("Red"); } ;
    }
}
