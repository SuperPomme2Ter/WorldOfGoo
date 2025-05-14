using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransporterManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int nbTransporters;
    [SerializeField] private GameObject transportersPrefab;
    [SerializeField] private StationManager stationManager;
    void Start()
    {
        stationManager.UpdateStationList();
        List<Cell> stations = stationManager.allStations.Select(a => a.GetComponent<Cell>()).ToList();
        for (int i = 0; i < nbTransporters; i++) 
        {
            GameObject newTransporterGO=Instantiate(transportersPrefab,transform.position,Quaternion.identity,transform);
            Transporters newTransporter = newTransporterGO.GetComponent<Transporters>();
            newTransporter.DeployTransporter += LaunchStationConstruction;
            newTransporter.SetSpawnCell(stations[0]?? null);
            
        }
    }

    void LaunchStationConstruction(Vector3 transporterPos)
    {
        nbTransporters--;
        stationManager.CreateStation(transporterPos);
        
        
    }
    
}
