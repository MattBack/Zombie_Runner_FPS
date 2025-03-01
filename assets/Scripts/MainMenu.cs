using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadScene(int sceneId) {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone) {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingScreen.SetActive(true);

            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }

    public void StartGame() {
        chosenLevel = (int)gameManager.levelSelector;
        LoadScene(chosenLevel);
        mainOptionsMenuUI.SetActive(false);
        levelSelectMenuUI.SetActive(false);

        Debug.Log("Selected Level is" + chosenLevel); // TODO: Remove
    }

    public void TitleMenu() {
        mainMenuTitleScreenUI.SetActive(true);
        mainOptionsMenuUI.SetActive(false);
    }

    public void PlayerCountMenu() {
        playerCountMenuUI.SetActive(true);
        mainMenuTitleScreenUI.SetActive(false);
        mainOptionsMenuUI.SetActive(false);
    }

    public void PlayerCountBackButton() {
        playerCountMenuUI.SetActive(false);
        mainMenuTitleScreenUI.SetActive(true);
    }

    public void CharacterSelectMenu() {
        characterSelectMenuUI.SetActive(true);
        playerCountMenuUI.SetActive(false);
    }

    public void CharacterSelectBackButton()
    {
        playerCountMenuUI.SetActive(true);
        characterSelectMenuUI.SetActive(false);
    }

    public void LevelSelectMenu()
    {
        levelSelectMenuUI.SetActive(true);
        characterSelectMenuUI.SetActive(false);

        Debug.Log("Choose a level (Haunted Woods, Stormy Mountains, Living in a Ghost Town (Cove) or Abandond Base"); // TODO: Remove
    }

    public void LevelSelectBackButton()
    {
        levelSelectMenuUI.SetActive(false);
        characterSelectMenuUI.SetActive(true);
    }

    public void OptionsMenu() {
        mainMenuTitleScreenUI.SetActive(false);
        mainOptionsMenuUI.SetActive(true);
    }

    public void AudioMenu()
    {
        mainOptionsMenuUI.SetActive(false);
        audioMenuUI.SetActive(true);
    }

    public void AudioMenuBackButton()
    {
        mainOptionsMenuUI.SetActive(true);
        audioMenuUI.SetActive(false);
    }

    public void ControlsMenu()
    {
        mainOptionsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
        audioMenuUI.SetActive(false);
    }

    public void ControlsMenuBackButton()
    {
        mainOptionsMenuUI.SetActive(true);
        controlsMenuUI.SetActive(false);
        audioMenuUI.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quitting Game..."); // TODO: Remove
    }
}
