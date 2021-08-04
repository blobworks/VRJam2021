using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 


public class RockButton : MonoBehaviour
{
    public enum ButtonType { Snap, Permadeath, Hand }
    [SerializeField] public ButtonType type; 
    [SerializeField] TMP_Text buttonText; 

    [SerializeField] Material buttonIdle; 
    [SerializeField] Material buttonTouch; 
    [SerializeField] Material buttonPressed;

    [SerializeField] MeshRenderer buttonRenderer;

    [SerializeField] public OVRInput.Controller controller; 
    
    GameManager gameManager; 
    HapticManager hapticManager; 

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
        hapticManager = FindObjectOfType<HapticManager>(); 
        UpdateText(); 
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<RockButton>())
        {
            if(other.GetComponent<RockButton>().type == ButtonType.Hand)
            {
                if(type == ButtonType.Snap)
                {
                    hapticManager.VibrateStandard(0.5f, 0.5f, 0.1f, other.GetComponent<RockButton>().controller); 
                    gameManager.snapOff = !gameManager.snapOff; 
                    UpdateText();
                }

                else if(type == ButtonType.Permadeath)
                {
                    hapticManager.VibrateStandard(0.5f, 0.5f, 0.1f, other.GetComponent<RockButton>().controller); 
                    gameManager.permaDeath = !gameManager.permaDeath; 
                    UpdateText();
                }
            }
        }
    }

    void UpdateText()
    {
        if(type == ButtonType.Hand) return; 

        if(type == ButtonType.Snap)
        {
            if(!gameManager.snapOff)
            {
                buttonText.text = "SNAP ON"; 
                buttonRenderer.material = buttonIdle; 

            }
            else
            {
                buttonText.text = "SNAP OFF"; 
                buttonRenderer.material = buttonPressed; 
            }
        }

        if(type == ButtonType.Permadeath)
        {
            if(gameManager.permaDeath)
            {
                buttonText.text = "PERMADEATH";
                buttonRenderer.material = buttonPressed; 
            }
            else
            {
                buttonText.text = "INFINITE LIVES"; 
                buttonRenderer.material = buttonIdle; 
            }
        }
    }
}
