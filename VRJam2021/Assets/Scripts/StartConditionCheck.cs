using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConditionCheck : MonoBehaviour
{
    [SerializeField] Rigidbody rb; 
    // [SerializeField] public bool started; 
    
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Return)) 
        {
            rb.useGravity = true; 
            rb.isKinematic = false; 
            print("START"); 
            gameManager.gameStarted = true; 
        }
    }
}
