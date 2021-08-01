using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFollow : MonoBehaviour
{
    bool stopFollow, coolDown; 

    [SerializeField] ParticleSystem waterFX; 

    void OnTriggerStay(Collider other) 
    {
        if(!stopFollow && other.GetComponent<AstronautControls>())
        {
            transform.position = other.transform.position;

            // if(!coolDown)
            // {
            // coolDown = true; 
            // Invoke("RemoveCoolDown", 0.2f); 
            // } 

            if(!waterFX.isPlaying)
            {
                other.GetComponent<AstronautControls>().boatFollow = this; 
                waterFX.Play(); 
            }
        }
    }

    void RemoveCoolDown()
    {
        coolDown = false; 
    }

    public void Detach()
    {
        stopFollow = true; 
        waterFX.Stop(); 
    }

    void OnTriggerExit(Collider other) 
    {
        if(!other.GetComponent<AstronautControls>() && !coolDown)
        {
            stopFollow = true; 
            waterFX.Stop(); 

        }
    }
}
