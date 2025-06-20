﻿#pragma warning disable CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject optionsMenuUI;
    [SerializeField] GameObject controlsMenuUI;
    [SerializeField] GameObject audioMenuUI;

    private PlayerInput playerInput;
    private InputAction pauseAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];
    }

    private void OnEnable()
    {
        pauseAction.performed += OnPausePressed;
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.performed -= OnPausePressed;
        pauseAction.Disable();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                
                Pause();
                Time.timeScale = 0f;
            }
        }

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        audioMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        audioMenuUI.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameIsPaused = true;
    }

    public void OptionsMenu()
    {
        pauseMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        audioMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameIsPaused = true;
    }

    public void ControlsMenu()
    {
        optionsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
        audioMenuUI.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameIsPaused = true;
    }

    public void AudioMenu()
    {
        optionsMenuUI.SetActive(false);
        audioMenuUI.SetActive(true);
        controlsMenuUI.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");
    }
}
