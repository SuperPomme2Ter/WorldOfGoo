using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StationManager : MonoBehaviour
{

    
    public GameObject stationPrefab;
    public GameObject linkPrefab;    
    public List<GameObject> allStations = new();
    public List<CellAttraction> stationsChecked;
    public event Action<List<GameObject>,CellAttraction> OnStationCreated;

    // Start is called before the first frame update
    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    //     else
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
    void Start()
    {
        UpdateStationList();
    }

    internal void CreateStation(Vector3 spawnPos)
    {
        GameObject cell = Instantiate(stationPrefab, transform.position, Quaternion.identity,transform);
        cell.GetComponent<StationCell>().StationInit(allStations);
        UpdateStationList();
        OnStationCreated?.Invoke(allStations, cell.GetComponent<CellAttraction>());
        return;
    }

    public void UpdateStationList()
    {
        allStations.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            allStations.Add(transform.GetChild(i).gameObject);
        }
    }
}
