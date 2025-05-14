using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Transporters : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    float distMax;
    List<GameObject> nearbyStations=new List<GameObject>();
    [SerializeField] List<GameObject> pivots=new List<GameObject>();
    [SerializeField] GameObject highlightSelection;
    [SerializeField] SpriteRenderer mainSprite;
    [SerializeField] SpriteRenderer highlightSprite;
    
    CircleCollider2D detectionRange;
    Vector3 originalPosition;
    TransporterPathfinding pathfinding;
    
    internal event Action<Vector3> DeployTransporter;

    
    void Start()
    {
        pathfinding = GetComponent<TransporterPathfinding>();
    }

    internal void SetSpawnCell(Cell spawnCell)
    {
        _ = spawnCell ?? throw new InvalidOperationException("SpawnCell cannot be null, transporter not happy");
        pathfinding.actualCell=spawnCell;
        pathfinding.destinationCell=spawnCell;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.pointerCurrentRaycast.worldPosition;
        nearbyStations.Clear();
        List<Collider2D> collider= Physics2D.OverlapCircleAll(transform.position, distMax).ToList();
        foreach (Collider2D col in collider.Where(x=> x.TryGetComponent<Cell>(out Cell trash)))
        {
            
            if (col is BoxCollider2D && Vector2.Distance(col.gameObject.transform.position,transform.position)<distMax)
            {
                if (nearbyStations.Count >= pivots.Count)
                {
                    break;
                }
                nearbyStations.Add(col.gameObject);
            }

        }
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
        highlightSelection.SetActive(true);
        mainSprite.sortingOrder += 5;
        highlightSprite.sortingOrder += 5;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        List<Collider2D> collider = Physics2D.OverlapCircleAll(transform.position, distMax).ToList();
        if (Physics2D.OverlapCircle(transform.position, 0.6f, 1 << 0))
        {
            transform.position = originalPosition;
            pathfinding.enabled = true;
            highlightSelection.SetActive(false);
            mainSprite.sortingOrder -= 5;
            highlightSprite.sortingOrder -= 5;
            return;
        }
        foreach (Collider2D col in collider.Where(x => x.TryGetComponent<Cell>(out Cell trash)))
        {

            if (col is BoxCollider2D && Vector2.Distance(col.gameObject.transform.position, transform.position) < distMax)
            {
                DeployTransporter?.Invoke(transform.position);
                //GameObject cell = Instantiate(Cell.stationPrefab, transform.position, Quaternion.identity,BeginningCell.instance.stationParent.transform);
                Destroy(gameObject);
                return;
                
            }
        }
        transform.position = originalPosition;
        pathfinding.enabled = true;
        highlightSelection.SetActive(false);
        mainSprite.sortingOrder -= 5;
        highlightSprite.sortingOrder -= 5;
    }
}
