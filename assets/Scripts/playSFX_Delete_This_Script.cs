using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSFX : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip shootSFX;
    public AudioSource playSfxSource;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            AudioSource audio = gameObject.AddComponent<AudioSource>();

            audio.PlayOneShot(shootSFX);
        }
    }
}
