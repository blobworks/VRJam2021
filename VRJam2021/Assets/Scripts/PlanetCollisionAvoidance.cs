    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollisionAvoidance : MonoBehaviour
{
    [SerializeField] GameObject[] tileSkins; 
    Vector3 enterPosition; 
    Quaternion enterRotation; 

    OVRGrabbable grabbable;

    public bool isGrabbable; 

    bool colliding; 

    Vector3 collisionPosition, grabbedPosition; 
    Quaternion collisionRotation, grabbedRotation; 

    public int currentCollision;  

    void Start()
    {
        RandomiseTileSet(); 
        if(GetComponent<OVRGrabbable>())
        {
            grabbable = GetComponent<OVRGrabbable>(); 
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
    }
}
