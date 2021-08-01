using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] float multiplier = 1f; 
    void Update()
    {
        transform.Rotate(Vector3.right * multiplier, Space.Self); 
    }
}
