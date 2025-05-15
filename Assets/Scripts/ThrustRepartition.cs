using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThrustRepartition : MonoBehaviour
{
    public List<CellAttraction> stationsChecked;
    [SerializeField] StationManager stationManager;
    List<GameObject> stationsList=new ();
    
    void Start()
    {
        stationManager.OnStationCreated += GetStationList;
    }


    private void GetStationList(List<GameObject> stations,CellAttraction newStation)
    {
        stationsList = stations;
        newStation.OnCallHelp += HelpListHandling;
    }
    
    
    public void HelpListHandling(CellAttraction stationToAdd,CellAttraction callingStation)
    {
        if (stationsChecked.Count+1 >= stationsList.Count)
        {
            foreach (CellAttraction station in stationsChecked)
            {
                station.supportThrust=Vector2.zero;
            }
            stationsChecked.Clear();
        }

        if (!stationsChecked.Contains(stationToAdd))
        {
            stationsChecked.Add(stationToAdd);
            return false;
        }
        return true;
    }
}
