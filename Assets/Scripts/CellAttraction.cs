using System.Collections.Generic;
using UnityEngine;


public class CellAttraction : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField]
    private Vector2 appliedGravity=new Vector2 (0,0);

    [SerializeField] private float aaaaa;
    private Vector2 Thrust;
    public float preciseWeight;

    [Header("Calculated by mass of the station * (10^thrustCoeff)")]
    [SerializeField] float thrustCoeff=1;

    private float maxThrust;
    private List<CelestialBody> attraction = new List<CelestialBody>();
    private GameObject pivot;
    float timer = 0;
    bool increment=true;
    private float ThrustMagnitude;
    
    public Vector2 excessAttraction;
    public Vector2 supportThrust;
    private float excessAttractionMagnitude;
    
    private GameObject actualPixel;
    
    [SerializeField] StationCell stationCell;
    List<StationCell> neightbourCells = new();
    private int updateCount = 0;
    [SerializeField] private int helpDirectionAngleMax = 30;
    

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pivot=transform.GetChild(0).gameObject;
        maxThrust=rb.mass*Mathf.Pow(10,thrustCoeff);
        
    }

<<<<<<< Updated upstream
    //Vector3 planetGravity;
=======
   // Vector3 planetGravity;
>>>>>>> Stashed changes

    private void FixedUpdate()
    {
        updateCount++;
        appliedGravity = GetAttractionValue();
        aaaaa = appliedGravity.magnitude;
        Thrust = Vector2.ClampMagnitude(-(appliedGravity+ rb.velocity)+supportThrust, maxThrust);
        float angle = Mathf.Atan2(Thrust.y, Thrust.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector2 scaleOfProp = new Vector2(Thrust.magnitude / maxThrust, Thrust.magnitude / maxThrust);

        if (scaleOfProp.magnitude != 0)
        {
            pivot.transform.GetChild(0).localScale = Vector3.Lerp(scaleOfProp, scaleOfProp / 1.5f, timer);
            if (increment)
            {
                timer += Time.deltaTime*15;
            }
            else
            {
                timer -= Time.deltaTime*15;
            }
            if(timer >= 1 || timer <=0)
            {
                increment=!increment;
            }
        }

        ThrustMagnitude=Thrust.magnitude;
        if (updateCount >= 10)
        {
            if (ThrustMagnitude >= maxThrust)
            {
                {
                    excessAttraction = -((appliedGravity+rb.velocity)+ supportThrust) - Thrust;
                    HelpSignal(excessAttraction);
                }
            }
            updateCount=0;
        }
        rb.AddForce((appliedGravity+Thrust),ForceMode2D.Force);

    }

    private Vector2 GetAttractionValue()
    {
        if (transform.position.x >= 256)
        {
            return Vector2.left * 1000;
        }

        if (transform.position.x < 0)
        {
            return Vector2.right * 1000;
        }

        if (transform.position.y >= 240)
        {
            return Vector2.down * 1000;
        }

        if (transform.position.y < 0)
        {
            return Vector2.up * 1000;
        }
        return S_GravityMap.pixelsForce[(int)transform.position.x][(int)transform.position.y];

    }
<<<<<<< Updated upstream
    
    
=======

    public void HelpSignal(Vector2 excessDirection)
    {
        Debug.Log($"Help launched for {gameObject.name}");
        for (int i = 0; i < stationCell.connections.Count; i++)
        {
            if (!stationCell.connections[i].TryGetComponent<StationCell>(out StationCell otherCell))
            {
                continue;
            }
            if (BeginningCell.instance.HelpListHandling(otherCell.GetComponent<CellAttraction>()))
            {
                neightbourCells.Add(otherCell);
            }
        }

        if (neightbourCells.Count <= 0)
        {
            return;
        }
        CellAttraction otherCellAttraction=null;
        for (int i = 0; i < neightbourCells.Count; i++)
        {
            otherCellAttraction= neightbourCells[i].GetComponent<CellAttraction>();
            if (Vector2.Angle(excessAttraction,otherCellAttraction.Thrust) < helpDirectionAngleMax)
            {
                otherCellAttraction.supportThrust = excessDirection;
            }
            BeginningCell.instance.HelpListHandling(otherCellAttraction);
        }
        neightbourCells.Clear();
    }
>>>>>>> Stashed changes
}