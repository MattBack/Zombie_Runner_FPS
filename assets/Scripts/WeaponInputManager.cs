using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInputManager : MonoBehaviour
{
    [SerializeField] private WeaponSwitcher weaponSwitcher;

    private PlayerInput playerInput;
    private InputAction fireAction;
    private InputAction reloadAction;
    private InputAction meleeAction;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        fireAction = playerInput.actions["Fire"];
        reloadAction = playerInput.actions["Reload"];
        meleeAction = playerInput.actions["Melee"];
    }

    private void OnEnable()
    {
        // Fire button events
        fireAction.started += OnFireStarted;
        fireAction.canceled += OnFireCanceled;

        // Other actions
        reloadAction.performed += OnReload;
        meleeAction.performed += OnMelee;

        fireAction.Enable();
        reloadAction.Enable();
        meleeAction.Enable();
    }

    private void OnDisable()
    {
        fireAction.started -= OnFireStarted;
        fireAction.canceled -= OnFireCanceled;

        reloadAction.performed -= OnReload;
        meleeAction.performed -= OnMelee;

        fireAction.Disable();
        reloadAction.Disable();
        meleeAction.Disable();
    }

    private void OnFireStarted(InputAction.CallbackContext context)
    {
        var weapon = weaponSwitcher.GetCurrentWeaponComponent();
        if (weapon != null)
        {
            weapon.OnFirePressed();
        }
    }

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        var weapon = weaponSwitcher.GetCurrentWeaponComponent();
        if (weapon != null)
        {
            weapon.OnFireReleased();
        }
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        var weapon = weaponSwitcher.GetCurrentWeaponComponent();
        if (weapon != null)
        {
            weapon.OnReloadPressed();
        }
    }

    private void OnMelee(InputAction.CallbackContext context)
    {
        var weapon = weaponSwitcher.GetCurrentWeaponComponent();
        if (weapon != null)
        {
            weapon.OnMeleePressed();
        }
    }
}
