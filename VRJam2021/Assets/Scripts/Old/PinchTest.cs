using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchTest : MonoBehaviour
{
    [SerializeField] public OVRHand leftHand; 
    [SerializeField] public OVRHand rightHand; 

    Transform rightIndexTip; 
    
    void Start()
    {
        // rightIndexTip = GameObject.Find("Hand_IndexTip").transform; 
    }

    void Update()
    {
        PinchCheck();
    }

    void PinchCheck()
    {
        if(leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            print("LEFT INDEX PINCHING");
        }
        if(rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            print("RIGHT INDEX PINCHING");
        }
    }
}
