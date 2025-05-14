using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Cell : MonoBehaviour
{
    public List<GameObject> connections = new();
    public Dictionary<SpringJoint2D, GameObject> springAndRenderer= new();
    public float minDistance;
    public float maxDistance;
    //public float spriteScale;
    [SerializeField] private GameObject linkPrefab;
    [SerializeField] private float springFrequency=1.5f;
    [Range(0,1)]
    [SerializeField] private float springDamping=1;
    

    public void LinkToAnotherCell(Cell anotherCell)
    {
        if (connections.Contains(anotherCell.gameObject))
        {
            Debug.Log("Station already linked");
            return;
        }

        SpringJoint2D newConnection = gameObject.AddComponent<SpringJoint2D>();
        GameObject linkGameobject = Instantiate(linkPrefab, transform.position, Quaternion.identity,transform);
        LineRenderer connectionsLink=linkGameobject.GetComponent<LineRenderer>();
        springAndRenderer.Add(newConnection,linkGameobject);
        int spriteCount = 0;
        List<Vector3> nbPos = new List<Vector3>();

            spriteCount = Mathf.CeilToInt(Vector3.Distance(anotherCell.transform.position, transform.position) / connectionsLink.textureScale.x);

            for (int i = 0; i < spriteCount; i++)
            {
                nbPos.Add((Vector3.Normalize(anotherCell.transform.position - transform.position) * i));
            }
        connectionsLink.positionCount = nbPos.Count;
        connectionsLink.SetPositions(nbPos.ToArray());
        if (connectionsLink.material != null)
            connectionsLink.material.mainTextureScale = new Vector2(connectionsLink.textureScale.x * spriteCount, 1);
        else
            Debug.LogError(name + "'s Line Renderer has no material!");
        newConnection.connectedBody = anotherCell.GetComponent<Rigidbody2D>();
        newConnection.dampingRatio = springDamping;
        newConnection.autoConfigureDistance = false;
        newConnection.distance=Mathf.Clamp(Vector2.Distance(anotherCell.transform.position, transform.position),minDistance,maxDistance);
        newConnection.frequency = springFrequency;
        anotherCell.connections.Add(gameObject);
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