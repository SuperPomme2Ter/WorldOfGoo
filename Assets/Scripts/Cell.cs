using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Cell : MonoBehaviour
{
    public List<GameObject> connections = new();
    public Dictionary<SpringJoint2D, GameObject> springAndRenderer= new Dictionary<SpringJoint2D, GameObject>();
    public float maxDistance;
    public float spriteScale;
    public Material linkMaterial;

    public void LinkToAnotherCell(Cell anotherCell)
    {
        if (connections.Contains(anotherCell.gameObject))
        {
            Debug.Log("Station already linked");
            return;
        }

        SpringJoint2D newConnection = gameObject.AddComponent<SpringJoint2D>();
        GameObject linkGameobject = Instantiate(BeginningCell.instance.linkPrefab, transform.position, Quaternion.identity,transform);
        LineRenderer connectionsLink=linkGameobject.GetComponent<LineRenderer>();
        springAndRenderer.Add(newConnection,linkGameobject);
        int spriteCount = 0;
        List<Vector3> nbPos = new List<Vector3>();

            spriteCount = Mathf.CeilToInt(Vector3.Distance(anotherCell.transform.position, transform.position) / spriteScale);

            for (int i = 0; i < spriteCount; i++)
            {
                nbPos.Add((Vector3.Normalize(anotherCell.transform.position - transform.position) * i));
            }
        connectionsLink.positionCount = nbPos.Count;
        connectionsLink.SetPositions(nbPos.ToArray());
        if (connectionsLink.material != null)
            connectionsLink.material.mainTextureScale = new Vector2(spriteScale * spriteCount, 1);
        else
            Debug.LogError(name + "'s Line Renderer has no material!");
        newConnection.connectedBody = anotherCell.GetComponent<Rigidbody2D>();
        newConnection.dampingRatio = 1;
        newConnection.distance = maxDistance;
        newConnection.frequency = 1.5f;
        anotherCell.connections.Add(this.gameObject);
        connections.Add(anotherCell.gameObject);
        LinkUpdater updater = linkGameobject.AddComponent<LinkUpdater>();
        updater.station1 = gameObject;
        updater.station2 = anotherCell.gameObject;

    }
    private void Update()
    {
        foreach (SpringJoint2D springs in GetComponents<SpringJoint2D>())
        {
            if(springAndRenderer.TryGetValue(springs,out GameObject child))
            child.GetComponent<LinkUpdater>().CreateLine();
        }
    }
}