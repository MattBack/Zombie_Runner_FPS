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
            //spawn Player_Prefab
            Transform playerSpawnPoint = PlayerSpawnPointsOnStart[i].transform;
            GameObject playerToSpawn = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);

            Debug.Log($"Spawned player: {i}");
        }
    }

    private IEnumerator DelayedPlayerSpawn()
    {
        yield return null;
        SpawnCharacters();
    }

}
