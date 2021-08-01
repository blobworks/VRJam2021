using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticManager : MonoBehaviour
{
    bool vibrating;
    
    public void VibrateAudio(AudioClip vibrationAudio, OVRInput.Controller controller)
    {
        OVRHapticsClip clip = new OVRHapticsClip(vibrationAudio); 

        if(controller == OVRInput.Controller.LTouch)
        {
            OVRHaptics.LeftChannel.Preempt(clip); 
        }
        else if(controller == OVRInput.Controller.RTouch)
        {
            OVRHaptics.RightChannel.Preempt(clip); 
        }
    }

    public void VibrationStop(OVRInput.Controller controller)
    {
        if(controller == OVRInput.Controller.LTouch || controller == OVRInput.Controller.RTouch || controller == OVRInput.Controller.Active)
        {
            // print("HAPTIC STOP"); 
            OVRInput.SetControllerVibration(0, 0, controller);
        }

    }

    public void VibrateStandard(float frequency = 0.5f, float amplitude = 0.5f, float duration = 0.5f , OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        // print("HAPTIC 2"); 
        StartCoroutine(Haptics(frequency, amplitude, duration, controller));
    }

    IEnumerator Haptics(float frequency = 0.5f, float amplitude = 0.5f, float duration = 0.5f , OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        if(controller == OVRInput.Controller.LTouch || controller == OVRInput.Controller.RTouch || controller == OVRInput.Controller.Active)
        {
            OVRInput.SetControllerVibration(frequency, amplitude, controller);
        }
        yield return new WaitForSeconds(duration);

        if(controller == OVRInput.Controller.LTouch || controller == OVRInput.Controller.RTouch)
        {
            // print("HAPTIC 4"); 
            OVRInput.SetControllerVibration(0, 0, controller);
        }
    }
}
