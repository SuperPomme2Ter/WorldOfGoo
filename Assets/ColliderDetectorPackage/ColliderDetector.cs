using System;
using UnityEngine;

public class ColliderDetector : MonoBehaviour
{
    internal Action<Collider2D> onTriggerEnterFunction= x => { return; } ;
    internal Action<Collider2D> onTriggerExitFunction= x => { return; } ;
    private void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerEnterFunction?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        onTriggerExitFunction?.Invoke(other);
    }
}
