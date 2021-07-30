using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class GoalDetector : MonoBehaviour
{
    [SerializeField] TMP_Text text; 
    [SerializeField] int nextLevelIndex; 
    public bool WinConditionMet; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) 
    {
        text.text = "WE MADE IT!"; 
        WinConditionMet = true; 
        Invoke("NextLevel", 2f); 
    }

    void NextLevel()
    {   
        SceneManager.LoadScene(nextLevelIndex); 
    }
}
