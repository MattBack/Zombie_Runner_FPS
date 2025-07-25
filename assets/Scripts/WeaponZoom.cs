using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] Camera fpsCamera;
    [SerializeField] RigidbodyFirstPersonController fpsController;
    [SerializeField] float zoomedOutFOV = 60f;
    [SerializeField] float zoomedInFOV = 20f;

    [Header("Zoom Visuals")]
    [SerializeField] Canvas zoomedInUICanvas;
    public Image zoomedInUIImage;
    public Sprite zoomedInUISprite;
    [Range(0, 1)] public float zoomedInTransparency = 0.5f;

    private PlayerInput playerInput;
    private InputAction zoomAction;

    private bool isSubscribed = false;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        zoomAction = playerInput.actions["Zoom"];

        zoomAction.Enable();
    }

    private void OnEnable()
    {
        ZoomOut();
        SubscribeInput();
    }

    private void OnDisable()
    {
        UnsubscribeInput();
        ZoomOut();
    }

    private void SubscribeInput()
    {
        if (!isSubscribed && zoomAction != null)
        {
            zoomAction.performed += OnZoomPressed;
            zoomAction.canceled += OnZoomPressed;
            isSubscribed = true;
        }
    }

    private void UnsubscribeInput()
    {
        if (isSubscribed && zoomAction != null)
        {
            zoomAction.performed -= OnZoomPressed;
            zoomAction.canceled -= OnZoomPressed;
            isSubscribed = false;
        }
    }

    private void Update()
    {
        if (zoomedInUICanvas != null && zoomedInUIImage != null)
        {
            Color color = zoomedInUIImage.color;
            color.a = zoomedInTransparency;
            zoomedInUIImage.color = color;
        }
    }

    public void OnZoomPressed(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            FindObjectOfType<AudioManager>().Play("WeaponZoomSfx");
            ZoomIn();
        }
        else if (context.canceled)
        {
            ZoomOut();
        }
    }

    private void ZoomIn()
    {
        fpsCamera.fieldOfView = zoomedInFOV;
        fpsController.mouseLook.SetZoomSensitivity();

        if (zoomedInUICanvas != null)
        {
            zoomedInUICanvas.enabled = true;
        }
    }

    private void ZoomOut()
    {
        fpsCamera.fieldOfView = zoomedOutFOV;
        fpsController.mouseLook.ResetSensitivity();

        if (zoomedInUICanvas != null)
        {
            zoomedInUICanvas.enabled = false;
        }
    }
}
