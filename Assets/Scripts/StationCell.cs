using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StationCell : Cell
{
    void Start()
    {
        BeginningCell beginning=BeginningCell.instance;
        beginning.allStations.Add(this.gameObject);
        maxDistance=beginning.maxDistance;
        spriteScale=beginning.spriteScale;
        linkMaterial = beginning.linkMaterial;
        foreach (GameObject Station in BeginningCell.instance.allStations.Where(x=>Vector2.Distance(x.transform.position,transform.position)<BeginningCell.instance.maxDistance)) 
        {
            if (connections.Count >= 3)
            {
                break;
            }
            Cell cell=Station.GetComponent<Cell>();
            if(Station!=gameObject)
            {
                LinkToAnotherCell(cell.GetComponent<Cell>());
            }
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            

        }
    }
}
