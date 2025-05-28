using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeapon = 0;
    [SerializeField] TwoBoneIKConstraint rightArmIK;
    [SerializeField] TwoBoneIKConstraint leftArmIK;  // Left arm IK constraint
    [SerializeField] RigBuilder rigBuilder; // Reference to RigBuilder

    private AudioManager audioManager;
    [Header("Animation")]
    private Animator weaponAnimator;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        weaponAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        SetWeaponActive();
    }

    void Update()
    {
        int previousWeapon = currentWeapon;

        ProcessKeyInput();
        ProcessScrollWheel();

        if (previousWeapon != currentWeapon)
        {
            SetWeaponActive();
        }
    }

    private void ProcessScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentWeapon >= GetValidWeaponCount() - 1)
            {
                currentWeapon = 0;
            }
            else
            {
                currentWeapon++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentWeapon <= 0)
            {
                currentWeapon = GetValidWeaponCount() - 1;
            }
            else
            {
                currentWeapon--;
            }
        }
    }

    private void ProcessKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentWeapon = 2;
    }

    private int GetValidWeaponCount()
    {
        int count = 0;
        foreach (Transform weapon in transform)
        {
            if (!weapon.name.StartsWith("ref_")) count++;
        }
        return count;
    }

    private void SetWeaponActive()
    {
        int weaponIndex = 0;

        foreach (Transform weapon in transform)
        {
            if (weapon.name.StartsWith("ref_")) continue;

            if (weaponIndex == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
                PlayDrawWeaponAnim();
                PlayDrawSound();

                Transform rightHandTarget = weapon.Find("ref_RightHandTarget");
                if (rightHandTarget != null) UpdateIKTarget(rightArmIK, rightHandTarget);

                Transform leftHandTarget = weapon.Find("ref_LeftHandTarget"); // Find left hand target
                if (leftHandTarget != null) UpdateIKTarget(leftArmIK, leftHandTarget);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }

    private void PlayDrawWeaponAnim()
    {
        weaponAnimator.Play("weaponDrawAnim");
    }

    private void PlayDrawSound()
    {
        if (audioManager != null) audioManager.Play("DrawWeaponSfx");
    }

    private void UpdateIKTarget(TwoBoneIKConstraint armIK, Transform newTarget)
    {
        armIK.data.target = newTarget;
        rigBuilder.Build();

        //Debug.Log("IK target updated to: " + newTarget.name);
    }
}
