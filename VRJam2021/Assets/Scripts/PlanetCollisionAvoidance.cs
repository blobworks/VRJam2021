﻿    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollisionAvoidance : MonoBehaviour
{
    
    Vector3 enterPosition; 
    Quaternion enterRotation; 

    OVRGrabbable grabbable;

    public bool isGrabbable; 

    bool colliding; 

    Vector3 collisionPosition, grabbedPosition; 
    Quaternion collisionRotation, grabbedRotation; 

    public int currentCollision;  

    void Start()
    {
        if(GetComponent<OVRGrabbable>())
        {
            grabbable = GetComponent<OVRGrabbable>(); 
        }
    }

    void Update()
    {
        if(grabbable == null) return; 

        if(grabbable.isGrabbed && grabbedPosition == Vector3.zero)
        {
            grabbedPosition = transform.position; 
            grabbedRotation = transform.rotation; 

            // Invoke("ResetZero", 0.1f); 
        }

        if(colliding && !grabbable.isGrabbed)
        {
            Snap(); 
        } 

        if(!colliding && !grabbable.isGrabbed && grabbedPosition != Vector3.zero)
        {
            ResetZero(); 
        }
        
    }

    public void Collide(bool colliding)
    {
        if(colliding)
        {
            colliding = true; 
        }
        else
        {
            colliding = false; 
        }
    }

    void ResetZero()
    {
        grabbedPosition = Vector3.zero; 
    }

    public void Check()
    {
        if(grabbable == null) return; 

        if(!grabbable.isGrabbed & grabbedPosition != Vector3.zero)
        {
            Snap(); 
        }
    }

    void Snap()
    {
        transform.position = grabbedPosition; 
        transform.rotation = grabbedRotation; 

        currentCollision = 0;

        grabbedPosition = Vector3.zero; 
    }
}
