using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection; 
using UnityEngine.SceneManagement; 

public class PersistentObject : MonoBehaviour
{
    [SerializeField] GameObject persistentObject; 

    bool spawned; 

    void Awake()
    {
        if(FindObjectsOfType<GameManager>().Length == 0)
        {
            Instantiate(persistentObject, this.transform); 
            spawned = true; 
            DontDestroyOnLoad(this.gameObject); 
        }
    }
}
