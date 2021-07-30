using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using TMPro; 

public class AstronautControls : MonoBehaviour
{
    // [SerializeField] TMP_Text fuelUsed; 
    [SerializeField] Rigidbody rb; 
    [SerializeField] StartConditionCheck start;
    
    [SerializeField] ParticleSystem jetPack; 

    [SerializeField] float coolDownTime = 2f; 

    [SerializeField] float maxCount; 

    float timeSinceBoosted, amountFuelUsed; 

    GameManager gameManager; 

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameStarted && BoostActivated() && ReadyToBoost())
        {
            timeSinceBoosted = Time.time; 

            gameManager.fuelSpent += Time.deltaTime; 
            gameManager.DataUpdate(); 

            jetPack.Emit(transform.position, Vector3.down + new Vector3(UnityEngine.Random.Range(-0.1f,0.1f), 0f,UnityEngine.Random.Range(-0.1f,0.1f)), 1f, 2f, Color.white);

            // fuelUsed.text = "Fuel " + Math.Round(amountFuelUsed, 2); 
            rb.AddForce(Vector3.up * 0.026f, ForceMode.VelocityChange);             
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
