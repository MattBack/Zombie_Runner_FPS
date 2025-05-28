using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCrouch : MonoBehaviour
{
    public float crouchHeight = 1f;
    public float crouchSpeed = 2f; 

    private CharacterController controller;
    private float originalHeight;
    private float originalSpeed;
    private FPS_RigidbodyFirstPersonController movement;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        movement = GetComponent<FPS_RigidbodyFirstPersonController>();
        originalHeight = controller.height;
        originalSpeed = movement.movementSettings.CurrentTargetSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
