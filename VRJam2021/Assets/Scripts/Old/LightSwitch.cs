using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] GameObject[] lights; 

    [SerializeField] Material standBy; 
    [SerializeField] Material click; 
    [SerializeField] Material defaultMaterial; 

    bool colliding, activated; 

    bool activeCoolDown; 

    int currentLightIndex; 

    public bool buttonTest; 
    
    OVRHand leftHand, rightHand; 

    OVRHand collidingHand; 

    Light currentLight; 
    
    float lightIntensity; 


    void Start()
    {

        leftHand = FindObjectOfType<InputManager>().skeletonLeft.GetComponent<OVRHand>(); 
        rightHand = FindObjectOfType<InputManager>().skeleton.GetComponent<OVRHand>(); 
    }

    void Update()
    {
        ButtonCheck();

        if(buttonTest)
        {
            ButtonPress();
            buttonTest = false; 
        }

        LightDimmer(); 
    }

    void ButtonCheck()
    {
        if(colliding && !activeCoolDown)
        {
            if(collidingHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                ButtonPress(); 
            }
        }
    }

    void ButtonPress()
    {
        activeCoolDown = true; 
        activated = true; 
        Invoke("DeactivateCoolDown", 2f); 
        SwitchLight(); 
    }

    void LightDimmer()
    {
        if(activated)
        {
            if(lightIntensity == 0)
            {
                lightIntensity = currentLight.intensity; 
                currentLight.intensity = 0;
            }

            currentLight.intensity +=  5f * Time.deltaTime;

            if(currentLight.intensity >= lightIntensity)
            {
                currentLight.intensity = lightIntensity; 
                activated = false; 
                lightIntensity = 0; 
                
            if(colliding)
            {
                ChangeMaterial(standBy); 
            }
            else
            {
                ChangeMaterial(defaultMaterial); 
            }
                print("RESET"); 
            }  
            print("LIGHT LERP " + currentLight.intensity); 
        } 
    }

    void SwitchLight()
    {
        foreach(GameObject light in lights)
        {
            light.SetActive(false); 
        }

        lights[currentLightIndex].SetActive(true); 
        currentLight = lights[currentLightIndex].GetComponent<Light>(); 

        ChangeMaterial(click); 

        currentLightIndex += 1; 
        if(currentLightIndex > lights.Length - 1)
        {
            currentLightIndex = 0; 
        }
    }

    void ChangeMaterial(Material passMaterial)
    {
        Material[] getMaterials = GetComponent<MeshRenderer>().materials; 

        Material[] newMat = getMaterials; 

        newMat[0] = getMaterials[0]; 
        newMat[1] = passMaterial; 

        GetComponent<MeshRenderer>().materials = newMat;         
    }



    void DeactivateCoolDown()
    {
        activeCoolDown = false; 
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            if(other.GetComponent<OVRHand>())
            {
                ChangeMaterial(standBy); 
                collidingHand = other.GetComponent<OVRHand>(); 
                colliding = true; 
            }
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            ChangeMaterial(defaultMaterial); 
            colliding = false; 
        }
    }

    
}
