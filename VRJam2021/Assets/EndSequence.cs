using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class EndSequence : MonoBehaviour
{
    [SerializeField] TMP_Text totalTimeText; 
    [SerializeField] TMP_Text fuel; 

    [SerializeField] GameObject sequenceOne; 
    [SerializeField] GameObject credits; 

    GameManager gameManager; 
    SoundManager soundManager; 

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>(); 
        soundManager.Stop(); 
        soundManager.Play(soundManager.calmMusic); 
        gameManager = FindObjectOfType<GameManager>(); 

        totalTimeText.text = "Roll Time: "+ gameManager.totalTime; 
        fuel.text = "Fuel Used: " + gameManager.fuelSpent; 

        Invoke("DisableOne", 10f); 
        Invoke("EnableCredits", 10f); 
        
    }

    void DisableOne()
    {
        sequenceOne.SetActive(false); 
        credits.SetActive(true); 
    }
}
