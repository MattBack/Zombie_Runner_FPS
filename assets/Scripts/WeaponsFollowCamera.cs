using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsFollowCamera : MonoBehaviour
{
    [SerializeField] Transform cameraTransform; // Assign your Main Camera in the Inspector
    [SerializeField] float rotationSpeed = 10f; // Adjust for smoother rotation

    private void Update()
    {
        // Smoothly interpolate the weapon's rotation to match the camera's rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraTransform.rotation, Time.deltaTime * rotationSpeed);
    }
}


