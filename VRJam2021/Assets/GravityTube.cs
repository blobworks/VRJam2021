using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTube : MonoBehaviour
{
    public enum Type { Entrance, Exit}; 
    [SerializeField] Type type; 
    [SerializeField] Transform enter; 
    [SerializeField] Transform exit; 

    // Start is called before the first frame update
    void Start()
    {
        enter.GetComponent<MeshRenderer>().enabled = false; 
        exit.GetComponent<MeshRenderer>().enabled = false; 
    }

    void OnTriggerEnter(Collider other) 
    {
            Rigidbody rb; 
            if(other.GetComponent<AstronautControls>())
            {
                rb = other.GetComponent<Rigidbody>(); 
                if(type == Type.Entrance)
                {
                    rb.velocity = Vector3.zero; 
                    rb.useGravity = false; 
                    rb.AddForce(exit.position - enter.position, ForceMode.Impulse); 
                }
                else if(type == Type.Exit)
                {
                    rb.useGravity = true; 
                    rb.AddForce(exit.position - enter.position, ForceMode.Impulse);
                }
            }
    }
}
