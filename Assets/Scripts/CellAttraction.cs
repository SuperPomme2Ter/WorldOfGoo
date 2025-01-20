using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CellAttraction : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector2 appliedGravity=new Vector2 (0,0);
    private Vector2 Thrust;
    [SerializeField] private float maxThrust;
    private List<CelestialBody> attraction = new List<CelestialBody>();
    private GameObject pivot;
    float timer = 0;
    bool increment=true;
    private float ThrustMagnitude;
    private GameObject actualPixel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pivot=transform.GetChild(0).gameObject;
        
    }

    Vector3 planetGravity;

    private void FixedUpdate()
    {
        appliedGravity = GetAttractionValue();
        Thrust = Vector2.ClampMagnitude(-(appliedGravity+ rb.velocity), maxThrust);
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
        rb.AddForce((appliedGravity+Thrust),ForceMode2D.Force);

    }

    private Vector2 GetAttractionValue()
    {
        if (transform.position.x >= 256)
        {
            return Vector2.left * 100;
        }

        if (transform.position.x < 0)
        {
            return Vector2.right * 100;
        }

        if (transform.position.y >= 240)
        {
            return Vector2.down * 100;
        }

        if (transform.position.y < 0)
        {
            return Vector2.up * 100;
        }
        return S_GravityMap.pixelsForce[(int)transform.position.x][(int)transform.position.y];

    }
}