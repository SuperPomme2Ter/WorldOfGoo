using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Transporters : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    BeginningCell Cell;
    float distMax;
    List<GameObject> nearbyStations=new List<GameObject>();
    List<GameObject> pivots=new List<GameObject>();
    CircleCollider2D detectionRange;
    Vector3 originalPosition;
    TransporterPathfinding pathfinding;

    void Start()
    {
        pathfinding=GetComponent<TransporterPathfinding>();
        Cell=BeginningCell.instance;
        distMax=Cell.maxDistance;
        pivots.Add(transform.GetChild(0).gameObject);
        pivots.Add(transform.GetChild(1).gameObject);
        
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.pointerCurrentRaycast.worldPosition;
        nearbyStations.Clear();
        List<Collider2D> collider= Physics2D.OverlapCircleAll(transform.position, distMax).ToList();
        foreach (Collider2D col in collider.Where(x=> x.TryGetComponent<Cell>(out Cell trash)))
        {
            
            if (col is CircleCollider2D && Vector2.Distance(col.gameObject.transform.position,transform.position)<distMax)
            {
                Debug.Log(col.gameObject.name);
                if (nearbyStations.Count >= pivots.Count)
                {
                    break;
                }
                nearbyStations.Add(col.gameObject);
            }

        }
        Debug.Log(nearbyStations.Count);
        for (int i = 0; i < nearbyStations.Count; i++)
        {
                pivots[i].gameObject.SetActive(true);
                Vector2 dir = nearbyStations[i].transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                pivots[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        if (nearbyStations.Count < pivots.Count) {
            for (int i = nearbyStations.Count; i < pivots.Count; i++)
            {
                pivots[i].gameObject.SetActive(false);
            }
        }
        

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originalPosition=transform.position;
        pathfinding.enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        List<Collider2D> collider = Physics2D.OverlapCircleAll(transform.position, distMax).ToList();
        if (Physics2D.OverlapCircle(transform.position, 0.6f, 1 << 0))
        {
            transform.position = originalPosition;
            pathfinding.enabled = true;
            return;
        }
        foreach (Collider2D col in collider.Where(x => x.TryGetComponent<Cell>(out Cell trash)))
        {

            if (col is CircleCollider2D && Vector2.Distance(col.gameObject.transform.position, transform.position) < distMax)
            {
                GameObject cell = Instantiate(Cell.stationPrefab, transform.position, Quaternion.identity);
                BeginningCell.instance.nbTransporters -= 1;
                Destroy(gameObject);
                return;
                
            }
        }
        transform.position = originalPosition;
        pathfinding.enabled = true;
    }
}
