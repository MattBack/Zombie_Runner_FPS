using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] GameObject gameOverCanvas;

    private void Start()
    {
        gameOverCanvas.SetActive(false);
    }

    public void HandleDeath()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;

        // Disable player actions
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        RigidbodyFirstPersonController playerMovement = FindObjectOfType<RigidbodyFirstPersonController>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Pause animations
        Animator[] animators = FindObjectsOfType<Animator>();
        foreach (Animator animator in animators)
        {
            animator.enabled = false;
        }

        // Stop audio
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioSources)
        {
            audio.Pause();
        }

        // Stop coroutines
        StopAllCoroutines();

        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
