using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource; 
    
    [SerializeField] public AudioClip calmMusic;
    [SerializeField] public AudioClip goTime;
    [SerializeField] public AudioClip gameOver;
    
    [SerializeField] public AudioClip jetPack; 
    [SerializeField] public AudioClip rocketBoost; 
    [SerializeField] public AudioClip bump; 

    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
        Stop();
        Play(calmMusic);  
    }

    public void PlayOne(AudioClip clip)
    {
        audioSource.spatialBlend = 0f; 
        audioSource.volume = 1f; 
        audioSource.PlayOneShot(clip); 
    }
    
    public void Play(AudioClip clip)
    {
        audioSource.spatialBlend = 0f; 
        audioSource.volume = 1f; 
        audioSource.PlayOneShot(clip); 
    }

    public void Stop()
    {
        audioSource.Stop(); 
    }
}
