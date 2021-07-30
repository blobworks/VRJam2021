using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseRandomly : MonoBehaviour
{
    PropActivator[] activators; 
    float timeSinceActivated; 

    [SerializeField] float triggerInterval = 1f; 

    void Start()
    {
        activators = FindObjectsOfType<PropActivator>();   

    }

    void Update() 
    {
        if(timeSinceActivated == 0)
        {
            timeSinceActivated = Time.time; 
        }

        if(Time.time > timeSinceActivated + triggerInterval)
        {
            activators[Random.Range(0,activators.Length)].Madness(); 
            timeSinceActivated = 0; 
        }
    }
}
