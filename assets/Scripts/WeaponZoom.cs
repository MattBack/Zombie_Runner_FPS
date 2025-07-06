using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WeaponZoom : MonoBehaviour
{
    internal object disabled;
    [SerializeField] Camera fpsCamera;
    [SerializeField] RigidbodyFirstPersonController fpsController;
    [SerializeField] float zoomedOutFOV = 60f;
    [SerializeField] float zoomedInFOV = 20f;
    [SerializeField] float zoomOutSensitivity = 2f;
    [SerializeField] float zoomInSensitivity = .5f;

    [Header("Zoom Visuals")]
    [SerializeField] Canvas zoomedInUICanvas; // canvas to toggle on and off
    [SerializeField] Image zoomedInUIImage; // image for the current weapon
    [SerializeField] Sprite zoomedInUISprite;
    [Range(0,1)] public float zoomedInTransparency = 0.5f;

    bool zoomedInToggle = false;

    private PlayerInput playerInput;
    private InputAction zoomAction;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        zoomAction = playerInput.actions["Zoom"];
    }

    private void Start()
    {
        ToggleZoomedInUI();
    }



    private void Update()
    {
        Color zoomedImageColor = zoomedInUIImage.color;
        zoomedImageColor.a = zoomedInTransparency;
        zoomedInUIImage.color = zoomedImageColor;



        //if (Input.GetMouseButtonDown(1))
        //{
        //    //FindObjectOfType<AudioManager>().Play("WeaponZoomSfx");

        //    //if (zoomedInToggle == false)
        //    //{
        //    //    ZoomIn();
        //    //}
        //    //else
        //    //{
        //    //    ZoomOut();
        //    //}
        //}
    }

    public void OnZoomPressed(InputAction.CallbackContext context)
    {
        FindObjectOfType<AudioManager>().Play("WeaponZoomSfx");

        if (zoomedInToggle == false)
        {
            ZoomIn();
        }
        else
        {
            ZoomOut();
        }
    }

    private void OnEnable()
    {
        zoomAction.performed += OnZoomPressed;
        zoomAction.Enable();
    }

    private void OnDisable()
    {
        ZoomOut();
        zoomAction.performed -= OnZoomPressed;
        zoomAction.Disable();
    }

    private void ZoomOut()
    {
        zoomedInToggle = false;
        fpsCamera.fieldOfView = zoomedOutFOV;
        fpsController.mouseLook.XSensitivity = zoomOutSensitivity;
        fpsController.mouseLook.YSensitivity = zoomOutSensitivity;

        ToggleZoomedInUI();
    }

    private void ZoomIn()
    {
        zoomedInToggle = true;
        fpsCamera.fieldOfView = zoomedInFOV;
        fpsController.mouseLook.XSensitivity = zoomInSensitivity;
        fpsController.mouseLook.YSensitivity = zoomInSensitivity;

        ToggleZoomedInUI();
    }

    private void SetZoomedInUIImage()
    {
        zoomedInUIImage.GetComponent<Image>().sprite = zoomedInUISprite;
    }

    private void ToggleZoomedInUI()
    {
        if (zoomedInUICanvas == null)
            return;

        if (!zoomedInToggle)
        {
            zoomedInUICanvas.enabled = false;
        }
        else
        {
            SetZoomedInUIImage();
            zoomedInUICanvas.enabled = true;
        }
    }
}
