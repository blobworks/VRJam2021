using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollision : MonoBehaviour
{
    PlanetCollisionAvoidance planetCollision; 
    [SerializeField] public MeshRenderer errorSphere; 

    void Start() 
    {
        planetCollision = GetComponentInParent<PlanetCollisionAvoidance>(); 
    }

    void Update()
    {
        
    }

    void OnTriggerStay(Collider other) 
    {
        errorSphere.enabled = true; 
    }

    void OnTriggerEnter(Collider other) 
    {
        errorSphere.enabled = true; 
    }

    void OnTriggerExit(Collider other)
    {
        errorSphere.enabled = false; 
    }
}
