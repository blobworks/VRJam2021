using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchControl : MonoBehaviour
{
    OVRHand leftHand, rightHand; 

    float pinchHeight; 
    bool rightPinched, leftPinched; 

    Vector3 pinchPos; 

    int locationIndex = 0; 

    bool oneIncreasing, twoIncreasing, threeIncreasing; 

    [Header("Benches")]
    [SerializeField] SkinnedMeshRenderer benchOne;
    [SerializeField] SkinnedMeshRenderer benchTwo;
    [SerializeField] SkinnedMeshRenderer benchThree;

    [Header("Locations")]
    [SerializeField] GameObject[] locationGameObjects; 

    [Header("Misc")]
    [SerializeField] GameObject[] benchesToDisable; 

    void Start()
    {
        leftHand = GetComponent<InputManager>().skeletonLeft.GetComponent<OVRHand>(); 
        rightHand = GetComponent<InputManager>().skeleton.GetComponent<OVRHand>(); 

        oneIncreasing = true; 
        twoIncreasing = true; 
    }

    void Update()
    {
        RightInputControl();

        LeftInputControl(); 
    }

    void RightInputControl()
    {
        float value; 

        if(rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            AdjustBench(OVRHand.HandFinger.Index); 
        }
        
        else if(rightHand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
        {
            AdjustBench(OVRHand.HandFinger.Middle); 
        }

        else if(rightHand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
        {
            AdjustBench(OVRHand.HandFinger.Ring); 
        }

        else if(rightPinched)
        {
            rightPinched = false; 
            print("pinch data reset"); 
        }
    }

    void LeftInputControl()
    {
        if(leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            if(!leftPinched)
            {
                leftPinched = true; 
                pinchPos = leftHand.transform.position; 
            }
        }
        else if(leftPinched && Vector3.Distance(pinchPos, leftHand.transform.position) < 0.1)
        {
            leftPinched = false; 
        }

        else if(leftPinched && Vector3.Distance(pinchPos, leftHand.transform.position) >= 0.1)
        {
            leftPinched = false; 
                        
            locationIndex += 1; 

            if(locationIndex >= locationGameObjects.Length)
            {
                locationIndex = 0; 
            }

            foreach(GameObject location in locationGameObjects)
            {
                location.SetActive(false); 
            }

            locationGameObjects[locationIndex].SetActive(true); 

            if(locationIndex == 2)
            {
                foreach(GameObject bench in benchesToDisable)
                {
                    bench.SetActive(false); 
                }
            }
            else
            {
                foreach(GameObject bench in benchesToDisable)
                {
                    bench.SetActive(true); 
                }
            }
        }


    }

    void AdjustBench(OVRHand.HandFinger finger)
    {
        SkinnedMeshRenderer benchToAdjust; 


        switch(finger)
        {
            default: 
            benchToAdjust = benchOne; 
            break; 

            case OVRHand.HandFinger.Index: 
            benchToAdjust = benchOne; 
            break; 
            
            case OVRHand.HandFinger.Middle: 
            benchToAdjust = benchTwo; 
            break; 
            
            case OVRHand.HandFinger.Ring: 
            benchToAdjust = benchThree; 
            break; 
        }

        if(!rightPinched)
        {
            rightPinched = true; 
            pinchHeight = rightHand.transform.position.y; 
        }

        if(rightHand.transform.position.y > pinchHeight + 0.01 && benchToAdjust.GetBlendShapeWeight(0) < 100)
        {
            benchToAdjust.SetBlendShapeWeight(0, benchToAdjust.GetBlendShapeWeight(0) + 1); 
        }

        else if(rightHand.transform.position.y < pinchHeight - 0.01 && benchToAdjust.GetBlendShapeWeight(0) > 0)
        {
            benchToAdjust.SetBlendShapeWeight(0, benchToAdjust.GetBlendShapeWeight(0) - 1); 
        }        
    }
}
