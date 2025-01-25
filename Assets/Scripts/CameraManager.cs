using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    InputManager inputManager;
    public Transform targetTransform;
    public Transform cameraPivot;

    private Vector3 cameraFollowVelocity = Vector3.zero;

    public float camerafollowSpeed = 0.2f;
    public float cameraLookSpeed = 0.3f;
    public float cameraPivotSpeed = 0.3f;

   

    public float lookAngle;
    public float pivotAngle;


    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;  
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
    }
    private void FollowTarget()
    {
        Vector3 targetposition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, camerafollowSpeed);
        
        transform.position = targetposition;
    }

    private void RotateCamera()
    {
        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
          

        pivotAngle = pivotAngle - ( inputManager.cameraInputY * cameraPivotSpeed);
  
        

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation=Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }
}
