using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject mainMenuTitleScreenUI;
    public GameObject mainOptionsMenuUI;
    public GameObject playerCountMenuUI;
    public GameObject characterSelectMenuUI;
    public GameObject levelSelectMenuUI;
    public GameObject audioMenuUI;
    public GameObject controlsMenuUI;

    public GameObject LoadingScreen;
    public Image LoadingBarFill;

    public GameManager gameManager;

    private int chosenLevel;

    [SerializeField] AudioClip buttonClick;

    AudioSource audioSource;

    [Header("First Selected Buttons")]
    public GameObject mainMenuFirstButton;
    public GameObject optionsMenuFirstButton;
    public GameObject playerCountFirstButton;
    public GameObject characterSelectFirstButton;
    public GameObject levelSelectFirstButton;
    public GameObject audioMenuFirstButton;
    public GameObject controlsMenuFirstButton;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingScreen.SetActive(true);

            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }

    public void StartGame()
    {
        chosenLevel = (int)gameManager.levelSelector;
        LoadScene(chosenLevel);
        mainOptionsMenuUI.SetActive(false);
        levelSelectMenuUI.SetActive(false);
        SetSelectedButton(mainMenuFirstButton);

        Debug.Log("Selected Level is" + chosenLevel); // TODO: Remove
    }

    public void TitleMenu()
    {
        mainMenuTitleScreenUI.SetActive(true);
        mainOptionsMenuUI.SetActive(false);
        SetSelectedButton(mainMenuFirstButton);

    }

    public void PlayerCountMenu()
    {
        playerCountMenuUI.SetActive(true);
        mainMenuTitleScreenUI.SetActive(false);
        mainOptionsMenuUI.SetActive(false);
        SetSelectedButton(playerCountFirstButton);
    }

    public void PlayerCountBackButton()
    {
        playerCountMenuUI.SetActive(false);
        mainMenuTitleScreenUI.SetActive(true);
        SetSelectedButton(mainMenuFirstButton);
    }

    public void CharacterSelectMenu()
    {
        characterSelectMenuUI.SetActive(true);
        playerCountMenuUI.SetActive(false);
        SetSelectedButton(characterSelectFirstButton);
    }

    public void CharacterSelectBackButton()
    {
        playerCountMenuUI.SetActive(true);
        characterSelectMenuUI.SetActive(false);
        SetSelectedButton(mainMenuFirstButton);
    }

    public void LevelSelectMenu()
    {
        levelSelectMenuUI.SetActive(true);
        characterSelectMenuUI.SetActive(false);
        SetSelectedButton(levelSelectFirstButton);

        Debug.Log("Choose a level (Haunted Woods, Stormy Mountains, Living in a Ghost Town (Cove) or Abandond Base"); // TODO: Remove
    }

    public void LevelSelectBackButton()
    {
        levelSelectMenuUI.SetActive(false);
        characterSelectMenuUI.SetActive(true);
        SetSelectedButton(mainMenuFirstButton);
    }

    public void OptionsMenu()
    {
        mainMenuTitleScreenUI.SetActive(false);
        mainOptionsMenuUI.SetActive(true);
        SetSelectedButton(optionsMenuFirstButton);
    }

    public void AudioMenu()
    {
        mainOptionsMenuUI.SetActive(false);
        audioMenuUI.SetActive(true);
        SetSelectedButton(audioMenuFirstButton);
    }

    public void AudioMenuBackButton()
    {
        mainOptionsMenuUI.SetActive(true);
        audioMenuUI.SetActive(false);
        SetSelectedButton(optionsMenuFirstButton);
    }

    public void ControlsMenu()
    {
        mainOptionsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
        audioMenuUI.SetActive(false);
        SetSelectedButton(controlsMenuFirstButton);
    }

    public void ControlsMenuBackButton()
    {
        mainOptionsMenuUI.SetActive(true);
        controlsMenuUI.SetActive(false);
        audioMenuUI.SetActive(false);
        SetSelectedButton(optionsMenuFirstButton);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game..."); // TODO: Remove
    }

    private void SetSelectedButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null); // clear previous
        EventSystem.current.SetSelectedGameObject(button);
    }
}
