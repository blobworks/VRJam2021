using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = transform.parent.transform.parent; 
        transform.localScale *= 10; 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
