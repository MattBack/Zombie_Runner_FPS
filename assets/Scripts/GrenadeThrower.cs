using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject grenadePrefab;

    [Header("Display Grenade Ammo")]
    [SerializeField] Ammo ammoSlot; // on the player
    [SerializeField] AmmoType ammoType;
    [SerializeField] TextMeshProUGUI ammoText;

    private int currentAmmo;

    // Controls Mapping
    private PlayerInput controls;
    private InputAction throwGrenadeAction;

    private void Awake()
    {
        controls = GetComponentInParent<PlayerInput>();
        throwGrenadeAction = controls.actions["ThrowGrenade"];
    }

    // Update is called once per frame
    void Update()
    {
        // Update the current ammo count
        currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);

        // Update the displayed ammo count
        DisplayGrenadeAmmo();

        // Check input and ammo before throwing a grenade
        //if (Input.GetKeyDown(KeyCode.Q) && currentAmmo > 0)
        //{
        //    OnThrowPressed();
        //    ReduceGrenadeAmmo();
        //}
    }

    public void OnThrowPressed(InputAction.CallbackContext context)
    {
        if (currentAmmo > 0)
        {
            GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * throwForce);
            ReduceGrenadeAmmo();
            // TODO: Add animation
        }
    }

    private void OnEnable()
    {
        throwGrenadeAction.performed += OnThrowPressed;
        throwGrenadeAction.Enable();
    }

    private void OnDisable()
    {
        throwGrenadeAction.performed -= OnThrowPressed;
        throwGrenadeAction.Disable();
    }

    //void ThrowGrenade()
    //{
    //    GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
    //    Rigidbody rb = grenade.GetComponent<Rigidbody>();
    //    rb.AddForce(transform.forward * throwForce);
    //    // TODO: Add animation
    //}

    private void DisplayGrenadeAmmo()
    {
        // Use the cached currentAmmo to update the UI
        ammoText.text = currentAmmo.ToString();
    }

    private void ReduceGrenadeAmmo()
    {
        // Reduce ammo in the Ammo script
        ammoSlot.ReduceCurrentAmmo(ammoType, 1);

        // Sync currentAmmo with the updated value
        currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
    }
}
