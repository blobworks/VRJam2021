using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVelocity : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody; 
    // Start is called before the first frame update
    void Start()
    {
        rigidbody.AddForce(new Vector3(Random.Range(-0.01f,0.01f), Random.Range(-0.01f,0.01f), Random.Range(-0.01f,0.01f)), ForceMode.Impulse);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
