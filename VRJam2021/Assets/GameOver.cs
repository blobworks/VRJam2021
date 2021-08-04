using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    GameManager gameManager; 
    SoundManager soundManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
        soundManager = FindObjectOfType<SoundManager>(); 
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
