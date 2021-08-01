using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using TMPro; 
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    [SerializeField] public int lives = 3; 
    
    [SerializeField] public GameObject currentAstronaut; 

    [SerializeField] public float fuelSpent;
    [SerializeField] public float timeSpent; 
    [SerializeField] public float totalTime; 
    [SerializeField] public float score; 

    [SerializeField] public float solutionPositionThreshold; 
    [SerializeField] public float solutionRotationThreshold; 

    [SerializeField] public bool gameStarted; 
    [SerializeField] public bool gameEnded; 

    [SerializeField] TMP_Text totalTimeText; 
    [SerializeField] TMP_Text fuelText; 
    [SerializeField] TMP_Text timeText; 
    [SerializeField] TMP_Text gameConditionText;


    public int[] timeKeep; 

    public float startTime;
    float lapsedTime;  

    bool scoreKept; 

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
        Reset(); 
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

        if(gameEnded && !scoreKept)
        {
            // lapsedTime = lapsedTime; 
            totalTime += lapsedTime; 
            totalTimeText.text = "Total Time: " + Math.Round(totalTime, 2); 
            gameConditionText.text = "WE DID IT!"; 
            scoreKept = true; 
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
        scoreKept = false; 
        gameConditionText.text = "HELP! (trigger to start)"; 
    }

    public void StartGame()
    {    
        gameStarted = true;
        startTime = Time.time;  
    }
}
