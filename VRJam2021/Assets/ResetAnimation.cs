using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimation : MonoBehaviour
{
    PlanetCollisionAvoidance planet; 

    bool resetDone; 

    void Start()
    {
        planet = GetComponentInParent<PlanetCollisionAvoidance>(); 
    }

    void Update()
    {
        if(planet.animationReset && !resetDone)
        {
            transform.localPosition = Vector3.zero; 
            resetDone = true; 
        }
    }
}
