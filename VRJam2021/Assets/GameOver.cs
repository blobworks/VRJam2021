using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    GameManager gameManager; 
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<AstronautControls>())
        {
            gameManager.gameOver = true; 
        }
    }
}
