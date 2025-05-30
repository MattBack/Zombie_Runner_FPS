using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDamage : MonoBehaviour
{
    [SerializeField] Canvas impactCanvas;
    [SerializeField] float impactTime = 0.3f;
    [SerializeField] AudioClip damageTakenAudioClip;
    public AudioSource damageTakenAudioSource;

    [SerializeField] Sprite[] damageSprites;
    private Image damageImage;

    void Start()
    {
        impactCanvas.enabled = false;
        damageImage = impactCanvas.GetComponentInChildren<Image>();
    }

    public void ShowDamageImpact()
    {
        //Debug.Log("SHOW DAMAGE");
        int randomIndex = UnityEngine.Random.Range(0, damageSprites.Length);
        damageImage.sprite = damageSprites[randomIndex];


        StartCoroutine(ShowSplatter());
    }

    IEnumerator ShowSplatter()
    {
        impactCanvas.enabled = true;
        playDamageTakenSound();
        yield return new WaitForSeconds(impactTime);
        impactCanvas.enabled = false;
    }

    private void playDamageTakenSound()
    {
        damageTakenAudioSource.PlayOneShot(damageTakenAudioClip);
        FindObjectOfType<AudioManager>().Play("PlayerTakeDamageSfx");
    }
}
