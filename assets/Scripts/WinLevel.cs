using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinLevel : MonoBehaviour
{
    [SerializeField] GameObject winMessage;
    [SerializeField] ParticleSystem winVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            winMessage.SetActive(true);
            PlayWinVisuals();
            Debug.Log("You Win, have a cookie =]");
            FindObjectOfType<AudioManager>().Play("TruckDriveSfx");
            Destroy(gameObject, 6f);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void PlayWinVisuals()
    {
       winVFX.Play();
    }
}
