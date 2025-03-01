using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform;  // Reference to the player's transform
    public Vector3 cameraOffset;       // Offset from the player's position
    public float positionDamping = 5f; // Damping factor for position interpolation
    public float rotationDamping = 10f; // Damping factor for rotation interpolation

    private Vector3 targetPosition;    // Desired camera position
    private Quaternion targetRotation; // Desired camera rotation

    private void LateUpdate()
    {
        // Calculate the target position and rotation based on the player's position and camera offset
        targetPosition = playerTransform.position + cameraOffset;
        targetRotation = Quaternion.Euler(playerTransform.eulerAngles.x, playerTransform.eulerAngles.y, 0f);

        // Smoothly interpolate the camera's position and rotation towards the target
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * positionDamping);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationDamping);
    }
}
