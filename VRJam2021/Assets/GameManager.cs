using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using TMPro; 
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    [SerializeField] public int lives = 3; 

    [SerializeField] public float fuelSpent;
    [SerializeField] public float timeSpent; 
    [SerializeField] public float score; 

    [SerializeField] public bool gameStarted; 
    [SerializeField] public bool gameEnded; 

    [SerializeField] TMP_Text fuelText; 
    [SerializeField] TMP_Text timeText; 
    [SerializeField] TMP_Text gameConditionText;

    public int[] timeKeep; 

    public float startTime;
    float lapsedTime;  

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
        gameConditionText.text = "HELP! (trigger to start)"; 
    }

    void Update()
    {
        if(gameStarted && !gameEnded)
        {
            gameConditionText.text = "HELP! (trigger to start)"; 
            lapsedTime = Time.time - startTime; 
            timeText.text = "Time: " + Math.Round(lapsedTime, 2); 

            fuelText.text = "Fuel spent: " +  Math.Round(fuelSpent, 2); 
        }

        if(gameEnded)
        {
            // lapsedTime = lapsedTime; 
            gameConditionText.text = "WE DID IT!"; 
        }
    }

    public void DataUpdate()
    {
        // I don't know what this is for
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Reset();
    }

    void Reset()
    {
        gameStarted = false; 
        gameEnded = false; 
    }
}
