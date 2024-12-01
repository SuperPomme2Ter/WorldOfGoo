using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransporterPathfinding : MonoBehaviour
{
    // Start is called before the first frame update
    BeginningCell Cell;
    Cell actualCell;
    Cell destinationCell;
    float travelDistance = 1;
    float speed;
    private void Start()
    {
        Cell = BeginningCell.instance;
        actualCell = Cell;
        destinationCell=Cell;
        
    }

    private void FixedUpdate()
    {
        if (actualCell == null || destinationCell == null)
        {
            actualCell = Cell;
            ChooseANeightbour();
        }
        if (travelDistance >= 1)
        {
            actualCell = destinationCell;
            ChooseANeightbour();
        }
        
        transform.position = Vector3.Lerp(actualCell.transform.position, destinationCell.transform.position, travelDistance);
        travelDistance += Time.deltaTime / speed;
        Vector2 dir = destinationCell.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void ChooseANeightbour()
    {
        if (actualCell.connections.Count > 0)
        {
            destinationCell = actualCell.connections[Random.Range(0, actualCell.connections.Count)].GetComponent<Cell>();
            speed = Random.Range(0.5f, 3);
            travelDistance = 0;
        }
    }
}
