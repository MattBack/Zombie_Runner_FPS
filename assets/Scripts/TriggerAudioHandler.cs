using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudioHandler : MonoBehaviour
{
    [Header("Sound config")]
    public AudioClip AudioClipToTrigger;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound()
    {
        if (audioSource != null && AudioClipToTrigger != null)
        {
            audioSource.clip = AudioClipToTrigger;
            audioSource.Play();
        }
    }

}
