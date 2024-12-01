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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pivot=transform.GetChild(0).gameObject;
        
    }

    Vector3 planetGravity;

    private void FixedUpdate()
    {
        appliedGravity=Vector2.zero;
        foreach (var planet in attraction)
        {
            Vector2 targetDirection = planet.transform.position - transform.position;
            targetDirection = targetDirection.normalized; // Normalize target direction vector
            targetDirection*=planet.gravityForce;
            //Quaternion rotation = Quaternion.FromToRotation(planet.gravity, targetDirection);
            //Vector2 rotatedFrom = rotation * planet.gravity;
            appliedGravity += targetDirection;
            
        }
        Thrust = Vector2.ClampMagnitude(-appliedGravity, maxThrust);
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
        rb.AddForce(rb.mass *(appliedGravity+Thrust),ForceMode2D.Force);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CelestialBody>(out CelestialBody planet))
        {
            if (!attraction.Contains(planet))
            attraction.Add(planet);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CelestialBody>(out CelestialBody planet))
        {
            if (!attraction.Contains(planet))
            attraction.Remove(planet);
        }
    }
}