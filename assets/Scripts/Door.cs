using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{

    public Camera DoorTrigger;
    public float MaxDistance = 10;

    public AudioClip doorCreak;

    private bool opened = false;

    private Animator anim;
    private AudioSource audioSource;

    private PlayerInput playerInput;
    private InputAction playerInteractAction;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerInteractAction = playerInput.actions["PlayerInteract"];
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        Pressed();
    //    }
    //}

    private void OnEnable()
    {
        playerInteractAction.performed += OnPlayerInteract;
        playerInteractAction.Enable();
    }

    private void OnDisable()
    {
        playerInteractAction.performed -= OnPlayerInteract;
        playerInteractAction.Disable();
    }

    public void OnPlayerInteract(InputAction.CallbackContext context)
    {
         Pressed();
    }

    void playDoorCreak() {
        audioSource.clip = doorCreak;
        audioSource.Play();
    }

    void Pressed()
    {
        RaycastHit doorHit;

        if (Physics.Raycast(DoorTrigger.transform.position, DoorTrigger.transform.forward, out doorHit, MaxDistance))
        {
            if (doorHit.transform.tag == "Door")
            {
                anim = doorHit.transform.GetComponentInParent<Animator>();
                playDoorCreak();

                opened = !opened;

                anim.SetBool("Opened", !opened);
            }
        }
    }

}
