using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro; 
using UnityEngine.SceneManagement; 

public class EndSequence : MonoBehaviour
{
    [SerializeField] TMP_Text totalTimeText; 
    [SerializeField] TMP_Text fuel; 

    [SerializeField] GameObject sequenceOne; 
    [SerializeField] GameObject credits; 
    [SerializeField] GameObject triggerToReturn; 

    GameManager gameManager; 
    SoundManager soundManager; 

    bool canReturn; 

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>(); 
        soundManager.Stop(); 
        soundManager.Play(soundManager.calmMusic); 
        gameManager = FindObjectOfType<GameManager>(); 

        totalTimeText.text = "Roll Time: "+ Math.Round(gameManager.totalTime, 2); 
        fuel.text = "Fuel Used: " + Math.Round(gameManager.fuelSpent, 2); 

        Invoke("DisableOne", 10f); 
        
    }

    void DisableOne()
    {
        sequenceOne.SetActive(false); 
        credits.SetActive(true);
        Invoke("ShowReturn", 5f);  
    }

    void ShowReturn()
    {
        triggerToReturn.SetActive(true); 
        canReturn = true; 
    }

    void Update()
    {
        if(canReturn)
        {
            if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) )
            {
                SceneManager.LoadScene(0); 
            }
        }
    }
}
