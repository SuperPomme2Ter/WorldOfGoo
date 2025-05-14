using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustRepartition : MonoBehaviour
{
    public List<CellAttraction> stationsChecked;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    public bool HelpListHandling(CellAttraction stationToAdd)
    {
        if (stationsChecked.Count+1 >= allStations.Count)
        {
            foreach (CellAttraction station in stationsChecked)
            {
                station.supportThrust=Vector2.zero;
            }
            stationsChecked.Clear();
            return false;
        }

        if (!stationsChecked.Contains(stationToAdd))
        {
            stationsChecked.Add(stationToAdd);
            return false;
        }
        return true;
    }
}
