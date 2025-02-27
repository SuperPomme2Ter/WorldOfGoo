using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeginningCell : Cell
{
    public static BeginningCell instance;
    [SerializeField] public int nbTransporters;
    [SerializeField] private GameObject transportersPrefab;
    public GameObject stationPrefab;
    public GameObject stationParent;
    public GameObject linkPrefab;    
    public List<GameObject> allStations = new();

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        allStations.Add(this.gameObject);
        for (int i = 0; i < nbTransporters; i++) 
        {
            Instantiate(transportersPrefab,transform.position,Quaternion.identity);
        }
        foreach (GameObject Station in allStations.Where(x=>Vector2.Distance(x.transform.position,transform.position)<maxDistance)) 
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
        }
        
    }
}
