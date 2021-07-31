using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

[System.Serializable]

public struct Gesture
{
    public string name; 
    public List<Vector3> fingerData; 
    public UnityEvent onRecognised; 
}

public enum ControllerMode
{
    Hands, Controllers
}

public enum MovementMode
{
    Glide, Teleport
}

public class InputManager : MonoBehaviour
{
    public float threshold = 0.1f; 
    public OVRSkeleton skeletonLeft;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures; 
    private List<OVRBone> fingerBones;
    private List<OVRBone> fingerBonesLeft;
    private Gesture previousGesture; 
    public CharacterController characterController; 
    [SerializeField] private LayerMask layerMask; 
    public Camera camera; 

    [SerializeField] int trajectorySegments = 10; 
    [SerializeField] float trajectorySpeed = 1f; 
    [SerializeField] float trajectoryCurve = 1f; 

    bool movementRotationReset; 
    LineRenderer lineRenderer; 

    public ControllerMode controllerMode; 
    public MovementMode movementMode; 

    [SerializeField] SkinnedMeshRenderer controllerRendererRight;  
    [SerializeField] SkinnedMeshRenderer controllerRendererLeft; 
    
    
    [SerializeField] GameObject teleportPoint; 

    GameObject spawnTeleportPoint; 

    SkinnedMeshRenderer handRendererLeft, handRendererRight; 

    Vector3 teleportPos = Vector3.zero; 
    
    bool leftForward, leftBackwards, rightForward, rightBackwards; 


    void Start()
    {
        spawnTeleportPoint = Instantiate(teleportPoint); 
        spawnTeleportPoint.SetActive(false); 
        lineRenderer = GetComponent<LineRenderer>(); 
        // controllerMode = ControllerMode.Hands; 
        previousGesture = new Gesture(); 
        if(controllerMode == ControllerMode.Hands)
        {
            handRendererLeft = skeletonLeft.GetComponent<SkinnedMeshRenderer>();
            handRendererRight = skeleton.GetComponent<SkinnedMeshRenderer>(); 
        }

    }

    void Update()
    {
        // GetBones(); 
        // HandConfidence();
        ControllerDetection(); 

        ControllerMovement(); 

        // if(skeleton.transform.childCount > 0 )
        // {
        //     Recognise();
        //     RecogniseLeft();
        //     HandAction(); 
        // }


        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     print("Right Hand Gesture Saved.");
        //     Save(); 
        // }

        // if(Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     print("Left Hand Gesture Saved");
        //     Save(true); 
        // }
    
    }

    void ControllerMovement()
    {

        if(controllerMode != ControllerMode.Controllers) return; 
        
        Vector3 direction; 

        if(movementMode == MovementMode.Teleport)
        {
            if(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Active).y > 0.5f)
            {
                TeleportTrajectory(true, controllerRendererLeft.transform.parent); 
            }

            if(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Active).y == 0f)
            {
                TeleportTrajectory(false); 
            }
        }

        else if(movementMode == MovementMode.Glide)
        {
            direction = new Vector3(camera.transform.forward.x, 0f, camera.transform.forward.z); 

            if(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Active).y > 0.5f)
            {
                characterController.SimpleMove(direction); 
                print("MOVING");
            }
            if(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Active).y < -0.5f)
            {
                characterController.SimpleMove(-direction); 
            }

        }

        if(!movementRotationReset)
        {
            if(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.Active).x > 0.5f)
            {
                characterController.transform.Rotate(0f, 90f, 0f); 
                movementRotationReset = true; 
            }
            if(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.Active).x < -0.5f)
            {
                characterController.transform.Rotate(0f, -90f, 0f); 
                movementRotationReset = true; 
            }
        }
        else
        {
            if(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.Active).x == 0f)
            {
                movementRotationReset = false; 
            }
        }
    }

    void TeleportTrajectory(bool turnOn = true, Transform posTransform = null, Vector3 front = default(Vector3))
    {
        if(turnOn)
        {
            lineRenderer.enabled = true; 
            RaycastHit hit; 
            float stepSize = 1.0f / trajectorySegments; 
            int segmentNumber = 0; 
            Vector3 pos, newPos; 

            // pos = new Vector3(0,1,0); 
            // Transform posTransform = controllerRendererLeft.transform.parent; 

            pos = posTransform.position; 
            newPos = Vector3.zero; 

            lineRenderer.positionCount = trajectorySegments;

            bool teleportPosFound = false; 

            for(float step = 0; step < 1; step += stepSize)
            {
                if(!teleportPosFound)
                {
                    lineRenderer.SetPosition(segmentNumber, pos);
                }
                
                else
                {
                    lineRenderer.SetPosition(segmentNumber, teleportPos);
                }

                if(front == default(Vector3))
                {
                    front = posTransform.forward; 
                }

                newPos = pos + front * trajectorySpeed+ (trajectoryCurve * new Vector3(0,-segmentNumber,0)) / trajectorySegments ; 

                if(Physics.Raycast(pos, newPos - pos, out hit, Vector3.Magnitude(newPos - pos), layerMask)) 
                {
                    print(hit.transform); 
                    teleportPos = hit.point; 
                    spawnTeleportPoint.transform.position = teleportPos; 
                    spawnTeleportPoint.SetActive(true); 
                    teleportPosFound = true; 
                }         

                pos = newPos; 

                segmentNumber += 1;  
            }
        }

        else
        {
            lineRenderer.enabled = false; 
            if(teleportPos != Vector3.zero)
            {
                characterController.enabled = false; 
                characterController.transform.position = teleportPos; 
                spawnTeleportPoint.SetActive(false); 
                characterController.enabled = true; 
                teleportPos = Vector3.zero; 
            }
        }

        // if(Physics.Raycast(controllerRendererLeft.transform.parent.position, controllerRendererLeft.transform.parent.forward, out hit, Mathf.Infinity, layerMask))
        // {
        //     lineRenderer.SetPosition(0, controllerRendererLeft.transform.parent.position);
        //     lineRenderer.SetPosition(1, hit.point); 
        // }
    }

    void ToggleControllers()
    {
        switch(controllerMode)
        {
            case ControllerMode.Hands: 

            handRendererLeft.enabled = false; 
            handRendererRight.enabled = false; 
            
            controllerRendererLeft.enabled = true; 
            controllerRendererRight.enabled = true; 
            controllerMode = ControllerMode.Controllers; 
            break; 

            case ControllerMode.Controllers: 
            handRendererLeft.enabled = true; 
            handRendererRight.enabled = true; 
            
            controllerRendererLeft.enabled = false; 
            controllerRendererRight.enabled = false; 

            controllerMode = ControllerMode.Hands; 
            break; 
        }
    }

    void ControllerDetection()
    {
        if(OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.Touch)) 
        {
            if(controllerMode == ControllerMode.Hands)
            {
                ToggleControllers(); 
            }
        }
    }

    void HandConfidence()
    {
        if(skeleton.transform.childCount > 0 && skeletonLeft.transform.childCount >= 0 && handRendererLeft.enabled && handRendererRight.enabled)
        {
            if(controllerRendererRight.enabled || controllerRendererLeft.enabled && controllerMode == ControllerMode.Controllers)
            {
                ToggleControllers();
            }
        }
    }

    void GetBones()
    {
        if(skeleton.transform.childCount <= 0 && skeletonLeft.transform.childCount <= 0) 
        {
            print("Hands not spawned yet");
            return;
        }

        else if(fingerBones == null && fingerBonesLeft == null)
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
            fingerBonesLeft = new List<OVRBone>(skeletonLeft.Bones);

            print("Hand bone assigned");
        }
    }

    void Save(bool leftHanded = false)
    {
        Gesture g = new Gesture(); 
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>(); 

        if(leftHanded)  
        {
            foreach( var bone in fingerBonesLeft )
            {
                data.Add(skeletonLeft.transform.InverseTransformPoint(bone.Transform.position)); 
            }
        }

        else 
        {
            foreach( var bone in fingerBones )
            {
                data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position)); 
            }
        }

        g.fingerData = data; 
        gestures.Add(g);  
        print("Gesture saved");
    }

    Gesture RecogniseLeft()
    {
        Gesture currentGesture = new Gesture(); 
        float currentMin = Mathf.Infinity; 

        foreach(var gesture in gestures)
        {
            float sumDistance = 0; 
            bool isDiscarded = false; 

            for(int i = 0; i < fingerBonesLeft.Count; i++)
            {
                Vector3 currentData = skeletonLeft.transform.InverseTransformPoint(fingerBonesLeft[i].Transform.position); 
                float distance = Vector3.Distance(currentData, gesture.fingerData[i]);
                if(distance > threshold)
                {
                    isDiscarded = true; 
                    // print(gesture.name + " does not match current pose"); 
                    break; 
                    // the current pose does not match a pre-saved gesture
                }  
                // else
                // {
                //     print("current gesture is "+ gesture.name); 
                // }
                sumDistance += distance; 
            }

            if(!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance; 
                currentGesture = gesture; 
                
                if(gesture.name == "LeftForward")
                {
                    leftForward = true; 
                }
                if(gesture.name == "LeftBackwards")
                {
                    leftBackwards = true; 
                }   
            }
        }
        // print(currentGesture.name);
        return currentGesture;
    }

    Gesture Recognise()
    {

        Gesture currentGesture = new Gesture(); 
        float currentMin = Mathf.Infinity; 

        foreach(var gesture in gestures)
        {
            float sumDistance = 0; 
            bool isDiscarded = false; 

            for(int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position); 
                float distance = Vector3.Distance(currentData, gesture.fingerData[i]);
                if(distance > threshold)
                {
                    isDiscarded = true; 
                    // print(gesture.name + " does not match current pose"); 
                    break; 
                    // the current pose does not match a pre-saved gesture
                }  
                // else
                // {
                //     print("current gesture is "+ gesture.name); 
                // }
                sumDistance += distance; 
            }

            if(!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance; 
                currentGesture = gesture; 
                
                if(gesture.name == "Forward")
                {
                    rightForward = true; 
                }
                if(gesture.name == "Backwards")
                {
                    rightBackwards = true; 
                }
                // Action(gesture); 
            }
        }
        // print(currentGesture.name);
        return currentGesture;
    }

    void HandAction()
    {   
        
        if(controllerMode != ControllerMode.Hands) return; 

        if(movementMode == MovementMode.Glide)
        {
            Vector3 direction; 
            direction = new Vector3(camera.transform.forward.x, 0f, camera.transform.forward.z); 

            if(rightForward && leftForward)
            {
                characterController.SimpleMove(direction); 
                // characterController.SimpleMove(direction / 40f); 
            }

            if(leftBackwards && rightBackwards)
            {
                characterController.SimpleMove(-direction); 
            }

            rightBackwards = false; 
            leftBackwards = false; 
            rightForward = false; 
            leftForward = false; 
        }

        else if(movementMode == MovementMode.Teleport)
        {
            if(rightForward && leftForward)
            {
                // Transform transform = new Transform; 
                TeleportTrajectory(true, skeletonLeft.transform, skeletonLeft.transform.right); 
                rightForward = false; 
                leftForward = false; 
            }
            else
            {
                TeleportTrajectory(false); 
            }
        }
    }

}
