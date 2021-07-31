using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GoalDetector : MonoBehaviour
{
    [SerializeField] GameObject rocket; 
    [SerializeField] Transform rocketSeat; 
    [SerializeField] int nextLevelIndex; 
    [SerializeField] ParticleSystem rocketFX;
    GameManager gameManager; 

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false; 
        gameManager = FindObjectOfType<GameManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            BeginNextLevel(); 
        }

        if(gameManager.gameEnded)
        {            
            rocket.transform.Translate(Vector3.up * 0.001f, Space.World);     
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        Rigidbody rb = other.GetComponent<Rigidbody>(); 
        
        rb.velocity = Vector3.zero; 
        rb.useGravity = false; 
        rb.angularVelocity = Vector3.zero; 
        other.transform.parent = rocket.transform; 
        other.transform.position = rocketSeat.position; 
        other.transform.rotation = rocketSeat.rotation; 
        BeginNextLevel(other);
    }

    void BeginNextLevel(Collider other = null)
    {
        rocketFX.Play(); 
        gameManager.gameEnded = true; 
        Invoke("NextLevel", 10f); 
    }    

    void NextLevel()
    {   
        SceneManager.LoadScene(nextLevelIndex); 
    }
}
