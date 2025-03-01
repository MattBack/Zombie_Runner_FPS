using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource playerWeaponSfx;
    public AudioClip bulletImpactSfx;

    public void PlayImpactSfx()
    {
        playerWeaponSfx.PlayOneShot(bulletImpactSfx);
    }
}
