using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkUpdater : MonoBehaviour
{
    public GameObject station1;
    public GameObject station2;

    public void CreateLine()
    {
        LineRenderer link =GetComponent<LineRenderer>();
        int spriteCount = 0;
        List<Vector3> nbPos = new List<Vector3>();

            spriteCount = Mathf.CeilToInt(Vector3.Distance(station2.transform.position, station1.transform.position)/ BeginningCell.instance.spriteScale);

            for (int i = 0; i < spriteCount; i++)
            {
                nbPos.Add(transform.position+((Vector3.Normalize(station2.transform.position - station1.transform.position) * i))* BeginningCell.instance.spriteScale);
            }

        
        link.positionCount = nbPos.Count;
        link.SetPositions(nbPos.ToArray());
        //link.material.mainTextureScale = new Vector2(spriteCount * BeginningCell.instance.spriteScale, 1);
        //if (link.material != null)
            //
        //else
         //   Debug.LogError(name + "'s Line Renderer has no material!");

    }
}
