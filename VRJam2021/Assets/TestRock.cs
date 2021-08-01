using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRock : MonoBehaviour
{
    [SerializeField] ParticleSystem destroyEffect; 
    OVRGrabbable grabbable; 
    Outline outline; 

    bool grabbed, grabbedRelease, hasBeenGrabbed; 
    OVRInput.Controller lastControllerType; 

    [SerializeField] GameObject tutorialObject; 

    GameManager gameManager; 
    Vector3 lastPosition;

    void Start()
    {
        outline = GetComponent<Outline>(); 
        gameManager = FindObjectOfType<GameManager>(); 
        grabbable = GetComponent<OVRGrabbable>(); 

        // Invoke("DestroyMe", 1f); 
    }

    void CheckPosition()
    {
        if(lastPosition == transform.position)
        {
            DestroyMe(); 
        }
        else
        {
            lastPosition = transform.position; 
            Invoke("CheckPosition", 2f); 
        }
    }

    void DestroyMe()
    {
        Destroy(this.gameObject); 
    }

    void Update()
    {

        if(grabbable == null) return;    

        if(grabbable.isGrabbed)
        {
            if(!grabbed)
            {
                hasBeenGrabbed = true; 
                DisableTutorial(); 
                CheckPosition(); 
                grabbed = true; 
                lastControllerType = grabbable.grabbedBy.controllerType; 
            }
        }
        
        if(grabbed && !grabbable.isGrabbed)
        {
            grabbedRelease = true;
            print("release detection one frame"); 
            grabbed = false; 
        }

        if(grabbedRelease)
        {
            // check pos for one frame
            if(true)
            {
                GetComponent<Rigidbody>().useGravity = true; 
                GetComponent<Rigidbody>().isKinematic = false; 
            }
            grabbedRelease = false; 
        }

        if(hasBeenGrabbed && gameManager.gameStarted)
        {
            DestroyMe(); 
        }

    }

    void DisableTutorial()
    {
        if(outline != null)
        {
            Destroy(outline);
        }
        if(tutorialObject != null)
        {
            tutorialObject.SetActive(false); 
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<GoalDetector>())
        {
            DestroyMe(); 
        }
    }

    void DestroyFX()
    {
        destroyEffect.transform.parent = null; 
        destroyEffect.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f); 
        destroyEffect.Play(); 
    }

    void OnDestroy() 
    {
        DestroyFX();
    }

}
