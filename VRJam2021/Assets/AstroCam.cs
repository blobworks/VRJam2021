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
        CamToggle(true); 
        gameManager = FindObjectOfType<GameManager>(); 
    }

    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch))
        {
            if(!camSphereToggled)
            {
                CamToggle(true); 
            }
            else
            {
                CamToggle(false); 
            }
        }
        
        if(camSphereToggled)
        {
            transform.position = rb.transform.position; 

            Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

            camSphere.parent.transform.Rotate(new Vector3(0, input.x, 0)); 
        }
    }

    void CamToggle(bool overShoulderCam)
    {
        if(overShoulderCam)
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
}
