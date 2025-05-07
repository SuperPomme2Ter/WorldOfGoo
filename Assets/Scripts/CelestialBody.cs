using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CelestialBody : MonoBehaviour
{
    public float weight;
    public float preciseWeight;
    [SerializeField] float intensityOffset=1;
    [SerializeField] private ColliderDetector planeteCollision;
    

    private void Start()
    {
        TryGetComponent<Light2D>(out Light2D light);
        if(light != null) { light.pointLightOuterRadius = transform.localScale.x * intensityOffset; }

        planeteCollision.onTriggerEnterFunction = CrashStations;

    }
    

    private void CrashStations(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Cell>(out Cell cell))
        {
            foreach (GameObject connections in cell.connections)
            {
                Cell cellConnection = connections.GetComponent<Cell>();
                Array springs=connections.GetComponents<SpringJoint2D>();
                foreach (SpringJoint2D spring in springs)
                {
                    if (spring.connectedBody.gameObject == collision.gameObject)
                    {
                        if (cellConnection.springAndRenderer.TryGetValue(spring, out GameObject childToDestroy))
                        {
                            cellConnection.springAndRenderer.Remove(spring);
                            Destroy(spring);
                            Destroy(childToDestroy);
                        }
                    }
                }
                
                cellConnection.connections.Remove(cell.gameObject);
            }
            BeginningCell.instance.stationsChecked.Remove(cell.GetComponent<CellAttraction>());
            BeginningCell.instance.allStations.Remove(cell.gameObject);
            Destroy(cell.gameObject);
        }
    }
}
