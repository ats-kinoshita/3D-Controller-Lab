using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    public Transform targetTransform;   // The object the camera will follow
    public Transform cameraPivot;       // The object the camera uses to pivot
    public Transform cameraTransform;   // The transform of the actual camera object in the scene 
    public LayerMask collisionLayers;   // The layers we want our camera to collide with
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public float cameraCollisionRadius = 0.2f;
    public float minCollisionOffset = 0.1f;
    public float cameraCollisionOffset = 0.2f;  // How much the camera jumps away from object it collides with
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 14f;
    public float cameraPivotSpeed = 14f;
    public float cameraLookSmoothTime = 1f;
    
    public float lookAngle; // Camera looking up and down
    public float pivotAngle; // Camera looding left and right
    public float minPivotAngle = -35f;
    public float maxPivotAngle = 35f;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();   // Need to make this smoother, less abrupt in terms of player controls
    }

    private void FollowTarget()
    {
        Vector3 targetPostion = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        
        transform.position = targetPostion;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;
        
        lookAngle = Mathf.Lerp
            (lookAngle, lookAngle + (inputManager.cameraInputX * cameraLookSpeed), cameraLookSmoothTime * Time.deltaTime);
        pivotAngle = Mathf.Lerp 
            (pivotAngle, pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed), cameraLookSmoothTime * Time.deltaTime);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset)
        {
            targetPosition =- minCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 10f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
