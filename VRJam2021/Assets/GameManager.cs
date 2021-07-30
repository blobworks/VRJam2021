using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int lives = 3; 

    [SerializeField] public float fuelSpent;
    [SerializeField] public float timeSpent; 
    [SerializeField] public float score; 

    [SerializeField] public bool gameStarted; 
    [SerializeField] public bool gameEnded; 

    public void DataUpdate()
    {
        // I don't know what this is for
    }
}
