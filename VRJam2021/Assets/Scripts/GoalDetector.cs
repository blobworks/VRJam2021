using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GoalDetector : MonoBehaviour
{
    [SerializeField] GameObject rocket; 
    [SerializeField] Transform rocketEntrance; 
    [SerializeField] Transform rocketSeat; 
    [SerializeField] int nextLevelIndex; 
    [SerializeField] ParticleSystem rocketFX;
    GameManager gameManager; 

    GameObject astronaught; 
    float enterDistance; 

    bool shipSummonSequence, rocketSeatSequence, takeOffSequence; 

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false; 
        gameManager = FindObjectOfType<GameManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // test 

        if(Input.GetKeyDown(KeyCode.G) || OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            NextLevel(); 
        }
        if(OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            ReloadThisLevel(); 
        }
          

        if(shipSummonSequence)
        {
            float ratio; 

            ratio = Vector3.Distance(astronaught.transform.position, rocketEntrance.position); 
            ratio = ratio / enterDistance; 
            if(ratio < 0.25f)
            {
                ratio = 0.25f; 
            }

            astronaught.transform.localScale = new Vector3(ratio, ratio, ratio); 

            if(Vector3.Distance(astronaught.transform.position, rocketEntrance.position) < 0.01f )
            {
                astronaught.transform.position = rocketSeat.position; 
                astronaught.GetComponent<Rigidbody>().velocity = Vector3.zero; 
                astronaught.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; 
                astronaught.transform.parent = rocket.transform; 
                astronaught.transform.rotation = rocketSeat.rotation; 
                shipSummonSequence = false; 
                rocketSeatSequence = true; 
            }

        }
        if(rocketSeatSequence)
        {
            if(astronaught.transform.localScale.x < 1.3)
            {
                astronaught.transform.localScale *= 1.02f; 
                print("ROCKET SCALE SEQUENCE"); 
                print(astronaught.transform.localScale); 
            }
            else
            {
                rocketSeatSequence = false; 
                Invoke("TakeOff", 1f); 
            }
        }
        else if(takeOffSequence)
        { 
            rocket.transform.Translate(Vector3.up * 0.001f, Space.World);  
        }
    }

    void TakeOff()
    {
        takeOffSequence = true; 

    }

    void OnTriggerEnter(Collider other) 
    {
        Rigidbody rb = other.GetComponent<Rigidbody>(); 

        shipSummonSequence = true; 
        
        rb.velocity = Vector3.zero; 
        rb.useGravity = false; 
        rb.AddForce(rocketEntrance.position - rb.transform.position, ForceMode.Impulse); 


        astronaught = rb.gameObject; 
        enterDistance = Vector3.Distance(astronaught.transform.position, rocketEntrance.position); 
        
        BeginNextLevel(other);
    }

    void BeginNextLevel(Collider other = null)
    {
        rocketFX.Play(); 
        gameManager.gameEnded = true; 
        Invoke("NextLevel", 10f); 
    }    

    void ReloadThisLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    void NextLevel()
    {   
        SceneManager.LoadScene(nextLevelIndex); 
    }
}
