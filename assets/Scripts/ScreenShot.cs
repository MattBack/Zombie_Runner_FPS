using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenShot : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction screenshotAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        screenshotAction = playerInput.actions["TakeScreenshot"];
    }

    ////public KeyCode screenShotButton;
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
            
    //    }
    //}

    private void OnEnable()
    {
        screenshotAction.performed += OnScreenshotPressed;
        screenshotAction.Enable();
    }

    private void OnDisable ()
    {
        screenshotAction.performed -= OnScreenshotPressed;
        screenshotAction.Enable();
    }

    public void OnScreenshotPressed(InputAction.CallbackContext context)
    {
            ScreenCapture.CaptureScreenshot("screenshot.png");
            Debug.Log("A screenshot was taken!");
    }
}