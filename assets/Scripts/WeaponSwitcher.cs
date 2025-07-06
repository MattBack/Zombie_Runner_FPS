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
    [Header("Animation")]
    private Animator weaponAnimator;

    private PlayerInput playerInput;
    //private InputAction switchWeaponAction;
    private InputAction scrollAction;

    private List<Transform> validWeapons = new List<Transform>();

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        scrollAction = playerInput.actions["ScrollWeapon"];
        scrollAction.performed += ScrollWeapon;
        //switchWeaponAction = playerInput.actions["SwitchWeapon"];
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
        //SetWeaponActive();
    }

    void Update()
    {

        //int previousWeapon = currentWeapon;

        ////ProcessKeyInput();
        ////ProcessScrollWheel();

        //if (previousWeapon != currentWeapon)
        //{
        //    SetWeaponActive();
        //}
    }

    private void OnEnable()
    {
        scrollAction.Enable();
        //switchWeaponAction?.Enable();
    }

    private void OnDisable()
    {
        scrollAction.performed -= ScrollWeapon;
        scrollAction.Disable();
        //switchWeaponAction?.Disable();
    }

    //private void ProcessScrollWheel()
    //{
    //    scrollTimer -= Time.deltaTime;
    //    if (scrollTimer > 0f) return;

    //    float scrollDelta = scrollAction.ReadValue<float>();
    //    Debug.Log($"Scroll Delta: {scrollDelta}");

    //    if (scrollDelta < -0.1f)
    //    {
    //        currentWeapon = (currentWeapon + 1) % GetValidWeaponCount();
    //        scrollTimer = scrollCooldown;
    //    }
    //    else if (scrollDelta > 0.1f)
    //    {
    //        currentWeapon--;
    //        if (currentWeapon < 0)
    //        {
    //            currentWeapon = GetValidWeaponCount() - 1;
    //        }
    //        scrollTimer = scrollCooldown;
    //    }
    //}

    private float scrollCooldown = 0.2f;
    private float scrollTimer = 0f;

    public void ScrollWeapon(InputAction.CallbackContext context)
    {
        if (Time.time < scrollTimer) return; // throttle fast scrolls
        scrollTimer = Time.time + scrollCooldown;

        float scrollDelta = context.ReadValue<float>();
        Debug.Log($"Scroll delta: {scrollDelta}");

        if (scrollDelta < -0.1f)
        {
            currentWeapon = (currentWeapon + 1) % GetValidWeaponCount();
        }
        else if (scrollDelta > 0.1f)
        {
            currentWeapon--;
            if (currentWeapon < 0)
                currentWeapon = GetValidWeaponCount() - 1;
        }

        SetWeaponActive();
    }
    //private void ProcessKeyInput()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1)) currentWeapon = 0;
    //    if (Input.GetKeyDown(KeyCode.Alpha2)) currentWeapon = 1;
    //    if (Input.GetKeyDown(KeyCode.Alpha3)) currentWeapon = 2;
    //}

    private int GetValidWeaponCount()
    {
        return validWeapons.Count;
    }

    private void SetWeaponActive()
    {
        for (int i = 0; i < validWeapons.Count; i++)
        {
            
            var weapon = validWeapons[i];
            if (i == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
                Debug.Log("Weapon active: " + weapon.name);
                PlayDrawWeaponAnim();
                PlayDrawSound();

                Transform rightHandTarget = weapon.Find("ref_RightHandTarget");
                if (rightHandTarget != null) UpdateIKTarget(rightArmIK, rightHandTarget);

                Transform leftHandTarget = weapon.Find("ref_LeftHandTarget");
                if (leftHandTarget != null) UpdateIKTarget(leftArmIK, leftHandTarget);
            }
            else
            {
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

        //Debug.Log("IK target updated to: " + newTarget.name);
    }
}
