using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropActivator : MonoBehaviour
{
    bool isActivated = false; 
    bool coolDown = false; 
    [SerializeField] float coolDownDuration = 1f; 
    [SerializeField] Animator animatorManual; 
    [SerializeField] Collider[] collidersToDisable; 

    Outline outline; 
    Animator animator; 
    InputManager inputManager; 

    [SerializeField] bool hasOutline; 
    [SerializeField] GameObject toOutline; 

    [SerializeField] bool testActivate; 

    // OVRHand leftHand, rightHand; 

    void Awake()
    {

            // leftHand = FindObjectOfType<PinchTest>().leftHand; 
            // rightHand = FindObjectOfType<PinchTest>().rightHand; 
            inputManager = FindObjectOfType<InputManager>(); 

            if(GetComponent<Animator>()) 
            {
                animator = GetComponent<Animator>(); 
            }
            else 
            {
                animator = animatorManual; 
            }

            if(animator.GetComponentInChildren<Outline>())
            {
                outline = animator.GetComponentInChildren<Outline>(); 
            }

            if(toOutline != null && hasOutline)
            {
                outline = toOutline.AddComponent<Outline>(); 
            }

            else if(hasOutline && outline == null)
            {
                outline = animator.gameObject.AddComponent<Outline>(); 
            }

            if(hasOutline)
            {
                outline.OutlineColor = Color.cyan; 
                outline.OutlineWidth = 4f; 
                outline.enabled = false; 
            }
    }

    void Update() 
    {
        if(testActivate)
        {
            Toggle();
            testActivate = false; 
        }        
    }

    public void Madness()
    {
        Toggle(); 
    }

    void OnTriggerStay(Collider other) 
    {
        // if(!other.transform.parent) return;   
        if(coolDown) return; 
        if(!other.transform.GetComponentInParent<HandGrabbingBehaviour>() && !other.transform.GetComponentInParent<OVRGrabber>()) return; 


        if(hasOutline)
        {
            outline.enabled = true; 
        }
        
        if(inputManager.controllerMode == ControllerMode.Hands)
        {
            if(other.GetComponent<HandGrabbingBehaviour>() && other.GetComponent<OVRHand>().GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                print(this.gameObject + " has been activated by hand " + other.gameObject );
                Toggle(); 
            }
        }

        if(inputManager.controllerMode == ControllerMode.Controllers)
        {
            if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, other.transform.GetComponentInParent<OVRGrabber>().ControllerType()))
            {
                print(this.gameObject + " has been activated by " + other.transform.GetComponentInParent<OVRGrabber>().ControllerType());
                Toggle(); 
            }
        }
        
    }

    void Toggle()
    {
        ToggleColliders(false); 
        if( !isActivated )
        {
            animator.SetTrigger("Activate"); 
            isActivated = true; 
        }
        else
        {
            animator.SetTrigger("Deactivate"); 
            isActivated = false; 
        }
        coolDown = true; 
        if(hasOutline)
        {
            outline.enabled = false; 
        }

        Invoke("CancelCoolDown", coolDownDuration); 
    }

    void ToggleColliders(bool state)
    {
        if(collidersToDisable.Length > 0)
        {
            foreach(Collider collider in collidersToDisable)
            {
                collider.GetComponent<Rigidbody>().isKinematic = state; 
            }
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if(!other.transform.GetComponentInParent<OVRGrabber>()) return; 

        if(hasOutline)
        {
            outline.enabled = false; 
        }
    }

    void CancelCoolDown()
    {
        ToggleColliders(true); 
        coolDown = false; 
    }
}
