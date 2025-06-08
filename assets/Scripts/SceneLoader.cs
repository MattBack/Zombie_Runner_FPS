using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //public Transform[] PlayerSpawnPointsOnStart;
    public GameObject playerPrefab; // TODO: update to come from selected character

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Prevent memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
        {
            Debug.Log($"Scene {scene.name} loaded, calling SpawnCharacters()");
            StartCoroutine(DelayedPlayerSpawn());
        }
    }

    public void ReloadGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1;
        StartCoroutine(DelayedPlayerSpawn());
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SpawnCharacters()
    {
        Debug.Log("Spawn Player called");

        int playerCount = (int)GameManager.Instance.numberOfPlayers;

        GameObject[] PlayerSpawnPointsOnStart = GameObject.FindGameObjectsWithTag("PlayerSpawnPoint");

        Debug.Log($"Found {PlayerSpawnPointsOnStart.Length} player spawn points in scene {SceneManager.GetActiveScene().name}");

        if (PlayerSpawnPointsOnStart == null || PlayerSpawnPointsOnStart.Length < playerCount)
        {
            Debug.LogError($"Not enough spawn points! Required: {playerCount}, Available: {PlayerSpawnPointsOnStart?.Length}");
            return;
        }

        for (int i = 0; i < playerCount;  i++)
        {
            Transform playerSpawnPoint = PlayerSpawnPointsOnStart[i].transform;
            GameObject playerToSpawn = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
            Debug.Log($"Spawned player: {i}");

            Camera playerCam = playerToSpawn.GetComponentInChildren<Camera>();
            if (playerCam != null)
            {
                playerCam.rect = GetViewportRect(i, playerCount);
            }
        }

        Rect GetViewportRect(int index, int totalPlayers)
        {
            switch (totalPlayers)
            {
                case 1:
                    return new Rect(0f, 0f, 1f, 1f);

                case 2:
                    return index == 0
                        ? new Rect(0f, 0.5f, 1f, 0.5f)   // Top half
                        : new Rect(0f, 0f, 1f, 0.5f);    // Bottom half

                case 3:
                    if (index == 0) return new Rect(0f, 0.5f, 0.5f, 0.5f);  // Top-left
                    if (index == 1) return new Rect(0.5f, 0.5f, 0.5f, 0.5f); // Top-right
                    return new Rect(0.25f, 0f, 0.5f, 0.5f);                 // Bottom-center

                case 4:
                    switch (index)
                    {
                        case 0: return new Rect(0f, 0.5f, 0.5f, 0.5f);  // Top-left
                        case 1: return new Rect(0.5f, 0.5f, 0.5f, 0.5f); // Top-right
                        case 2: return new Rect(0f, 0f, 0.5f, 0.5f);     // Bottom-left
                        case 3: return new Rect(0.5f, 0f, 0.5f, 0.5f);    // Bottom-right
                    }
                    break;
            }

            // Default fallback
            return new Rect(0f, 0f, 1f, 1f);
        }
    }

    private IEnumerator DelayedPlayerSpawn()
    {
        yield return null;
        SpawnCharacters();
    }



}
