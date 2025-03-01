using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footsteps : MonoBehaviour
{
    private AudioSource footstepSound;
    private bool IsMoving;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        footstepSound = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0) IsMoving = true;
        else IsMoving = false;

        if (IsMoving && !footstepSound.isPlaying)
        {
            footstepSound.Play();
            audioManager.Play("PlayerBreathing");
        }
        if (!IsMoving) footstepSound.Stop();


        }
        // TODO: if player moving forward?
           // play dirt footstep sound from audio manager
           // else player is not moving - stop sound
           // choose footstep sound at random mathf.random??
           // use tag to choose footstep sound
}
