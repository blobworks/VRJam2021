using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTube : MonoBehaviour
{
    public enum Type { Entrance, Exit}; 
    [SerializeField] Type type; 
    [SerializeField] Transform enter; 
    [SerializeField] Transform exit; 

    bool centering; 
    Rigidbody rb; 

    GameManager gameManager; 
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
        enter.GetComponent<MeshRenderer>().enabled = false; 
        exit.GetComponent<MeshRenderer>().enabled = false; 
    }

    void Update()
    {
        if(centering)
        {
            if(rb != null)
            {
                if(Vector3.Distance(rb.transform.position, enter.transform.position) < 0.02f)
                {
                    rb.transform.position = enter.transform.position; 
                    TravelUpTube(); 
                }
            }
        }
    }
    

    void OnTriggerEnter(Collider other) 
    {
            // if(other.GetComponent<AstronautControls>())
            if(true)
            {
                if(other.GetComponent<AstronautControls>())
                {
                    if(other.GetComponent<AstronautControls>().boatFollow != null)
                    {
                        other.GetComponent<AstronautControls>().boatFollow.Detach(); 
                    }
                }
                rb = other.GetComponent<Rigidbody>(); 
                if(type == Type.Entrance)
                {
                    CenterToTube(other); 
                    Invoke("GravityReset", 4f); 
                }
                else if(type == Type.Exit)
                {
                    rb.useGravity = true; 
                    rb.AddForce((exit.position - enter.position) * 0.5f, ForceMode.Impulse);
                }
            }
    }

    void CenterToTube(Collider other)
    {
        rb.velocity = Vector3.zero; 
        rb.useGravity = false; 
        centering = true; 
        rb.AddForce(enter.position - other.transform.position, ForceMode.Impulse); 
        
    }

    void TravelUpTube()
    {
        rb.velocity = Vector3.zero; 
        rb.useGravity = false; 
        centering = false; 
        rb.AddForce(exit.position - enter.position, ForceMode.Impulse); 
    }

    void GravityReset()
    {
        if(!gameManager.gameEnded)
        {
            rb.useGravity = true; 
        }
    }
}
