using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public Camera DoorTrigger;
    public float MaxDistance = 10;

    public AudioClip doorCreak;

    private bool opened = false;

    private Animator anim;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Pressed();
        }
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
