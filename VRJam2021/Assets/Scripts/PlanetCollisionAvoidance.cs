    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollisionAvoidance : MonoBehaviour
{
    [SerializeField] Vector3 solutionPosition;
    [SerializeField] Vector3 solutionRotation; 
    
    [SerializeField] ResetAnimation resetAnimation; 

    [SerializeField] Transform solution; 

    [SerializeField] GameObject[] tileSkins;

    [SerializeField] bool testSolution; 

    Vector3 enterPosition; 
    Quaternion enterRotation; 

    OVRGrabbable grabbable;
    
    [SerializeField] bool snapOn = true; 

    public bool isGrabbable; 

    bool colliding, grabbed, grabbedRelease; 

    Vector3 collisionPosition, grabbedPosition; 
    Quaternion collisionRotation, grabbedRotation; 

    public int currentCollision;  

    GameManager gameManager; 
    HapticManager hapticManager; 

    OVRInput.Controller lastControllerType; 

    public bool animationReset; 

    Animation animation; 

    void Start()
    {
        hapticManager = FindObjectOfType<HapticManager>(); 
        gameManager = FindObjectOfType<GameManager>(); 
        RandomiseTileSet(); 
        if(GetComponent<OVRGrabbable>())
        {
            grabbable = GetComponent<OVRGrabbable>(); 
        }

        DisableAnimationCheck(); 
    }

    void DisableAnimationCheck()
    {
        animation = GetComponent<Animation>(); 
        
        if(!grabbable.isGrabbable)
        {
            if(animation != null)
            {
                Destroy(animation); 
            }
        }
    }


    void RandomiseTileSet()
    {
        if(tileSkins.Length > 1)
        {
            foreach(GameObject tile in tileSkins)
            {
                tile.SetActive(false); 
            }

            int index = Random.Range(0, tileSkins.Length );

            print("TILE SKIN "+ index); 
            tileSkins[index].SetActive(true);  
        }
    }

    void Update()
    {
        if(grabbable == null) return;    

        if(testSolution)
        {
            animation.enabled = false; 
            animationReset = true; 
            transform.position = solutionPosition; 
            transform.eulerAngles = solutionRotation; 
            testSolution = false; 
        }

        if(grabbable.isGrabbed)
        {
            if(!grabbed)
            {
                if(animation != null)
                {
                    animation.enabled = false; 
                    animationReset = true; 
                }
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
            if(WithinSolutionRange() && !gameManager.snapOff)
            {
                transform.position = solutionPosition; 
                transform.eulerAngles = solutionRotation; 

                print("S N A P   T O   S O L U T I O N");
                hapticManager.VibrateStandard(0.5f, 0.5f, 0.1f, lastControllerType); 
            }
            grabbedRelease = false; 
        }

        if(gameManager.gameStarted && grabbable.isGrabbable)
        {
            grabbable.isGrabbable = false; 
        }
    }

    bool WithinSolutionRange()
    {
        print("ANGLE COMPARISON "+ Quaternion.Angle(transform.rotation, Quaternion.Euler(solutionRotation)) ); 
        if(Vector3.Distance(transform.position, solutionPosition) < gameManager.solutionPositionThreshold && Quaternion.Angle(transform.rotation, Quaternion.Euler(solutionRotation)) < gameManager.solutionRotationThreshold) return true; 
        else return false; 
    }
}
