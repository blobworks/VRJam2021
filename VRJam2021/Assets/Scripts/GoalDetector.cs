using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GoalDetector : MonoBehaviour
{
    [SerializeField] int nextLevelIndex; 
    GameManager gameManager; 

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            BeginNextLevel(); 
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        BeginNextLevel(); 
    }

    void BeginNextLevel()
    {
        gameManager.gameEnded = true; 
        Invoke("NextLevel", 2f); 
    }    

    void NextLevel()
    {   
        SceneManager.LoadScene(nextLevelIndex); 
    }
}
