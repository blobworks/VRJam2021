using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using TMPro; 

public class AstronautControls : MonoBehaviour
{
    [SerializeField] public Camera astroCam;
    // [SerializeField] TMP_Text fuelUsed; 
    [SerializeField] public Rigidbody rb; 
    [SerializeField] StartConditionCheck start;
    
    [SerializeField] ParticleSystem jetPack; 

    [SerializeField] float coolDownTime = 2f; 

    [SerializeField] float maxCount; 

    float timeSinceBoosted, amountFuelUsed; 

    [SerializeField] bool worldJetPack; 

    GameManager gameManager; 

    public int enteredTube; 

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 

        gameManager.currentAstronaut = transform.parent.gameObject; 

        if(start == null)
        {
            start = FindObjectOfType<StartConditionCheck>(); 
        }
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameStarted && BoostActivated() && ReadyToBoost())
        {
            Vector3 boostDirection; 

            timeSinceBoosted = Time.time; 

            gameManager.fuelSpent += Time.deltaTime; 
            gameManager.DataUpdate(); 

            rb.angularVelocity = Vector3.zero; 

            if(worldJetPack)
            {
                boostDirection = Vector3.up; 
            }
            else 
            {
                boostDirection = transform.up; 
            }
            

            jetPack.Emit(transform.position, -boostDirection + new Vector3(UnityEngine.Random.Range(-0.1f,0.1f), 0f,UnityEngine.Random.Range(-0.1f,0.1f)), 0.5f, 2f, Color.white);   

            if(rb.useGravity)
            {
                rb.AddForce(boostDirection * 0.026f, ForceMode.VelocityChange);   
            }          


        }   

        // if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Space)) 
        // {
        //     rb.AddForce(Vector3.up * 2f, ForceMode.Impulse); 
        // }
    }

    bool BoostActivated()
    {
        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            return true; 
        }

        if(Input.GetKey(KeyCode.Space))
        {
            return true; 
        }
        else return false; 
    }

    bool ReadyToBoost()
    {
        return true; 
        if(timeSinceBoosted == 0 || Time.time >= timeSinceBoosted + coolDownTime) 
        {
            return true;     
        }
        else return false; 
    }
}
