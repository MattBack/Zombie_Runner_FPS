using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioClip doorCreak;
    private bool opened = false;

    private Animator anim;
    private AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDoorCreak()
    {
        if (doorCreak != null)
        {
            audioSource.clip = doorCreak;
            audioSource.Play();
        }
    }

    public void ToggleDoor()
    {
        opened = !opened;
        anim.SetBool("Opened", opened);
        PlayDoorCreak();
    }
}
