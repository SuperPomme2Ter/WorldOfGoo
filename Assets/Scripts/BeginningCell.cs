using System.Collections.Generic;
using UnityEngine;

public class BeginningCell : Cell
{
    public static BeginningCell instance;
    [SerializeField] public int nbTransporters;
    [SerializeField] private GameObject transportersPrefab;
    public GameObject stationPrefab;
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
        
    }
}
