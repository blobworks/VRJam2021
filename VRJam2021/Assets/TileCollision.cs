using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollision : MonoBehaviour
{
    PlanetCollisionAvoidance planetCollision; 

    void Start() 
    {
        planetCollision = GetComponentInParent<PlanetCollisionAvoidance>(); 
    }

    void Update()
    {
        
    }

    void OnTriggerStay(Collider other) 
    {
        planetCollision.Check(); 
        // if(planetCollision.GetComponent<OVRGrabbable>())
        // {
        //     if(planetCollision.GetComponent<OVRGrabbable>().isGrabbed)
        //     {
        //         planetCollision.Check(); 
        //     }
        // }
    }

    void OnTriggerEnter(Collider other) 
    {
        planetCollision.Collide(true); 
    }

    void OnTriggerExit(Collider other)
    {
        planetCollision.Collide(false); 
    }
}
