using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeapon = 0;
    [SerializeField] TwoBoneIKConstraint rightArmIK;
    [SerializeField] TwoBoneIKConstraint leftArmIK;  // Left arm IK constraint
    [SerializeField] RigBuilder rigBuilder; // Reference to RigBuilder

    private AudioManager audioManager;
    private Animator weaponAnimator;

    private PlayerInput playerInput;
    private InputAction scrollAction;
    private InputAction nextWeaponAction;
    private InputAction previousWeaponAction;

    private List<Transform> validWeapons = new List<Transform>();

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        // Existing scroll input
        scrollAction = playerInput.actions["ScrollWeapon"];
        scrollAction.performed += ScrollWeapon;

        // New inputs for Next/Previous weapon
        nextWeaponAction = playerInput.actions["NextWeapon"];
        previousWeaponAction = playerInput.actions["PreviousWeapon"];
        nextWeaponAction.performed += HandleNextWeapon;
        previousWeaponAction.performed += HandlePreviousWeapon;

        audioManager = FindObjectOfType<AudioManager>();
        weaponAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        foreach (Transform weapon in transform)
        {
            if (!weapon.name.StartsWith("ref_"))
                validWeapons.Add(weapon);
        }

        StartCoroutine(InitializeWeapons());
    }

    private IEnumerator InitializeWeapons()
    {
        yield return null;
        SetWeaponActive();
    }

    private void OnEnable()
    {
        scrollAction.Enable();
        nextWeaponAction.Enable();
        previousWeaponAction.Enable();
    }

    private void OnDisable()
    {
        scrollAction.performed -= ScrollWeapon;
        nextWeaponAction.performed -= HandleNextWeapon;
        previousWeaponAction.performed -= HandlePreviousWeapon;

        scrollAction.Disable();
        nextWeaponAction.Disable();
        previousWeaponAction.Disable();
    }

    private float scrollCooldown = 0.2f;
    private float scrollTimer = 0f;

    public void ScrollWeapon(InputAction.CallbackContext context)
    {
        if (Time.time < scrollTimer) return; // Throttle fast scrolls
        scrollTimer = Time.time + scrollCooldown;

        float scrollDelta = context.ReadValue<float>();

        if (scrollDelta < -0.1f)
        {
            SelectNextWeapon();
        }
        else if (scrollDelta > 0.1f)
        {
            SelectPreviousWeapon();
        }
    }

    private void HandleNextWeapon(InputAction.CallbackContext context)
    {
        if (Time.time < scrollTimer) return; // Apply cooldown to avoid rapid switches
        scrollTimer = Time.time + scrollCooldown;

        SelectNextWeapon();
    }

    private void HandlePreviousWeapon(InputAction.CallbackContext context)
    {
        if (Time.time < scrollTimer) return; // Apply cooldown to avoid rapid switches
        scrollTimer = Time.time + scrollCooldown;

        SelectPreviousWeapon();
    }

    private void SelectNextWeapon()
    {
        currentWeapon = (currentWeapon + 1) % GetValidWeaponCount();
        SetWeaponActive();
    }

    private void SelectPreviousWeapon()
    {
        currentWeapon--;
        if (currentWeapon < 0)
            currentWeapon = GetValidWeaponCount() - 1;

        SetWeaponActive();
    }

    private int GetValidWeaponCount()
    {
        return validWeapons.Count;
    }

    private void SetWeaponActive()
    {
        for (int i = 0; i < validWeapons.Count; i++)
        {
            var weapon = validWeapons[i];
            var weaponZoom = weapon.GetComponent<WeaponZoom>();

            bool isActive = (i == currentWeapon);

            if (isActive)
            {
                weapon.gameObject.SetActive(true);
                if (weaponZoom != null)
                {
                    weaponZoom.enabled = true;

                    if (weaponZoom.zoomedInUIImage != null && weaponZoom.zoomedInUISprite != null)
                    {
                        weaponZoom.zoomedInUIImage.sprite = weaponZoom.zoomedInUISprite;
                    }
                }

                PlayDrawWeaponAnim();
                PlayDrawSound();

                Transform rightHandTarget = weapon.Find("ref_RightHandTarget");
                if (rightHandTarget != null) UpdateIKTarget(rightArmIK, rightHandTarget);

                Transform leftHandTarget = weapon.Find("ref_LeftHandTarget");
                if (leftHandTarget != null) UpdateIKTarget(leftArmIK, leftHandTarget);
            }
            else
            {
                if (weaponZoom != null) weaponZoom.enabled = false;
                weapon.gameObject.SetActive(false);
            }
        }
    }

    public Weapon GetCurrentWeaponComponent()
    {
        if (currentWeapon < 0 || currentWeapon >= validWeapons.Count) return null;
        return validWeapons[currentWeapon].GetComponent<Weapon>();
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
    }
}
