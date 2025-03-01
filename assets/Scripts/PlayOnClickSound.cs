using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayOnClickSound : MonoBehaviour
{
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip hoverButtonSound;
    [SerializeField] AudioClip playStartLevelSound;

    AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOnClick() {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);   
        }
        else {
            Debug.LogWarning("AudioClip or AudioSource is missing. Make sure to assign them in the Inspector.");
        }
    }

    public void PlayOnHoverButtonSound() {
        if (hoverButtonSound != null && audioSource != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(hoverButtonSound);
            }
            else {
                //audioSource.Stop();
                audioSource.PlayOneShot(hoverButtonSound);
            }
            
        }
        else {
            Debug.LogWarning("AudioClip or AudioSource is missing. Make sure to assign them in the Inspector.");
        }
    }

    public void PlayStartLevelSound()
    {
        if (playStartLevelSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(playStartLevelSound);
        }
        else
        {
            Debug.LogWarning("AudioClip or AudioSource is missing. Make sure to assign them in the Inspector.");
        }
    }
}
