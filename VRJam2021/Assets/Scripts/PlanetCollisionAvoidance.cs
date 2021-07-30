using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollisionAvoidance : MonoBehaviour
{
    Vector3 enterPosition; 
    Quaternion enterRotation; 

    OVRGrabbable grabbable; 

    void Start()
    {
        grabbable = GetComponent<OVRGrabbable>(); 
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.GetComponent<PlanetCollisionAvoidance>())
        {
            enterPosition = transform.position; 
            enterRotation = transform.rotation; 
            print("COLLISION DETECTED "+ this.gameObject + other.gameObject); 
        }    
    }

    void OnCollisionStay(Collision other) 
    {   
        if(!grabbable.isGrabbed && enterPosition != Vector3.zero)
        {
            transform.position = enterPosition; 
            transform.rotation = enterRotation; 
        }
    }
}
