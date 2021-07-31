using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroCam : MonoBehaviour
{

    GameManager gameManager; 
    [SerializeField] public Transform camSphere; 
    [SerializeField] public Transform povCam; 
    [SerializeField] Camera camera; 
    [SerializeField] Rigidbody rb;

    bool camSphereToggled; 

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
    }

    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
        {
            if(!camSphereToggled)
            {
                camera.transform.position = camSphere.position;
                camera.transform.rotation = camSphere.rotation;
                camera.transform.parent = camSphere;
                camSphereToggled = true; 
            }
            else
            {
                camera.transform.position = povCam.position; 
                camera.transform.rotation = povCam.rotation; 
                camera.transform.parent = povCam; 
                camSphereToggled = false; 
            }
        }
        
        if(camSphereToggled)
        {
            transform.position = rb.transform.position; 
        }
    }
}
