using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConditionCheck : MonoBehaviour
{
    [SerializeField] Rigidbody rb; 
    
    List<MeshRenderer> rendererlist = new List<MeshRenderer>(); 

    // [SerializeField] public bool started; 
    
    GameManager gameManager;
    SoundManager soundManager; 

    void Start()
    {
        FindErrorSpheres(); 
        GetComponent<MeshRenderer>().enabled = false; 
        gameManager = FindObjectOfType<GameManager>(); 
        soundManager = FindObjectOfType<SoundManager>(); 


        soundManager.Stop(); 
        soundManager.Play(soundManager.calmMusic); 

        rb = gameManager.currentAstronaut.GetComponentInChildren<AstronautControls>().rb; 
    }

    void FindErrorSpheres()
    {
        foreach(TileCollision tile in FindObjectsOfType<TileCollision>())
        {
            rendererlist.Add(tile.errorSphere); 
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.Return) ) 
        {
            if(ConditionMet())
            {
                rb.useGravity = true; 
                rb.isKinematic = false; 
                print("START"); 
                gameManager.StartGame(); 
                soundManager.Stop(); 
                soundManager.Play(soundManager.goTime); 
            } 
            else
            {
                print("CLEAR OBSTRUCTIONS"); 
            }
        }
    }

    bool ConditionMet()
    {
        if(gameManager.gameEnded) return false; 
        
        foreach(MeshRenderer renderer in rendererlist)
        {
            if(renderer.enabled)
            {
                return false; 
            }
            else 
            {
                return true; 
            }
        }
        return false; 
    }
}
