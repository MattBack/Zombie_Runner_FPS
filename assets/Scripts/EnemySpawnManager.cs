using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UrbanZombie_CharacterCustomize;

public class EnemySpawnManager : MonoBehaviour
{
    public int enemyCount = 0;
    public int enemySpawnCount = 10;
    public GameObject[] m_EnemyPrefabs;
    public GameEventsManager eventsManager;

    public Transform player; // Used to calculate distance to player
    public float spawnRange = 30f; // Max distance for valid spawn points
    public float minSpawnDistance = 10f; // Minimum distance away from the player
    public float spawnDelay = 2f; // Delay in seconds between each enemy spawn
    public float checkInterval = 5f; // How often to check and remove distant enemies
    public float destructionRange = 50f; // Range beyond which enemies are destroyed
    public int minEnemiesNearPlayer = 3; // Minimum enemies near the player before distant ones are removed

    private Transform[] m_SpawnPoints;
    private List<GameObject> activeEnemies = new List<GameObject>(); // List to track active enemies
    private int enemyPrefabCount;
    private bool isSpawning = false;

    public List<Transform> validSpawnPointsInInspector = new List<Transform>();

    void Start()
    {
        // Automatically find all spawn points in the scene by their tag
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        m_SpawnPoints = new Transform[spawnPointObjects.Length];

        for (int i = 0; i < spawnPointObjects.Length; i++)
        {
            m_SpawnPoints[i] = spawnPointObjects[i].transform;
        }

        enemyCount = 0;

        for (int i = 0; i < m_EnemyPrefabs.Length; i++)
        {
            m_EnemyPrefabs[i].GetComponent<EnemyHealth>().numberPrefab = eventsManager.numberPrefab;
        }

        enemyPrefabCount = m_EnemyPrefabs.Length;

        // Start the periodic check to remove distant enemies
        InvokeRepeating("CheckAndRemoveDistantEnemies", checkInterval, checkInterval);
    }

    void Update()
    {
        // Continuously check if enemies need to be spawned
        if (enemyCount < enemySpawnCount && !isSpawning)
        {
            StartCoroutine(SpawnEnemiesWithDelay());
        }

        // Continuously update the valid spawn points based on the player's movement
        UpdateValidSpawnPoints();
    }

    void UpdateValidSpawnPoints()
    {
        // Clear the list of valid spawn points
        validSpawnPointsInInspector.Clear();

        // Check each spawn point and see if it's within the player's range
        foreach (Transform spawnPoint in m_SpawnPoints)
        {
            if (spawnPoint != null)
            {
                float distanceToPlayer = Vector3.Distance(player.position, spawnPoint.position);

                // If the spawn point is within range and not directly in front of the player, add it
                if (distanceToPlayer <= spawnRange && !IsPointDirectlyInFrontOfPlayer(spawnPoint.position))
                {
                    validSpawnPointsInInspector.Add(spawnPoint);
                }
            }
        }
    }

    // Updated spawning logic using dynamic spawn points
    IEnumerator SpawnEnemiesWithDelay()
    {
        isSpawning = true; // Set the flag to indicate spawning is happening

        // Keep spawning enemies while we are below the maximum enemy count
        while (enemyCount < enemySpawnCount)
        {
            // Only proceed if we have valid spawn points
            if (validSpawnPointsInInspector.Count > 0)
            {
                // Pick a random spawn point from the valid ones
                int randomSpawnPointNumber = Random.Range(0, validSpawnPointsInInspector.Count);
                int randomEnemyPrefabNumber = Random.Range(0, m_EnemyPrefabs.Length);

                // Spawn the enemy at the chosen spawn point
                GameObject newEnemy = Instantiate(m_EnemyPrefabs[randomEnemyPrefabNumber],
                                                   validSpawnPointsInInspector[randomSpawnPointNumber].position,
                                                   Quaternion.identity);

                // Add the spawned enemy to the activeEnemies list
                activeEnemies.Add(newEnemy);

                enemyCount++;
                //Debug.Log($"Spawned new enemy: {newEnemy.name} at {newEnemy.transform.position}");
            }

            // Wait for the delay before spawning the next enemy
            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false; // Reset the flag after spawning completes
    }

    bool IsPointDirectlyInFrontOfPlayer(Vector3 spawnPosition)
    {
        Vector3 directionToSpawnPoint = (spawnPosition - player.position).normalized;
        float angle = Vector3.Angle(player.forward, directionToSpawnPoint);

        // Return true if the spawn point is within a narrow cone in front of the player
        return angle < 30f; // Adjust the angle as needed to suit your needs
    }

    void SpawnNewEnemy()
    {
        Transform[] validSpawnPoints = GetSpawnPointsInRange();

        // Ensure spawn points are valid
        if (validSpawnPoints.Length > 1)
        {
            int randomSpawnPointNumber = Random.Range(1, validSpawnPoints.Length);
            int randomEnemyPrefabNumber = Random.Range(0, enemyPrefabCount);

            GameObject newEnemy = Instantiate(m_EnemyPrefabs[randomEnemyPrefabNumber],
                         validSpawnPoints[randomSpawnPointNumber].position,
                         Quaternion.identity);

            activeEnemies.Add(newEnemy); // Add the spawned enemy to the list
            enemyCount++;

            // Debug to check if the enemy is added
            //Debug.Log($"Spawned new enemy: {newEnemy.name} at {newEnemy.transform.position}");
        }
    }

    Transform[] GetSpawnPointsInRange()
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in m_SpawnPoints)
        {
            float distanceToPlayer = Vector3.Distance(player.position, spawnPoint.position);

            if (distanceToPlayer <= spawnRange && distanceToPlayer >= minSpawnDistance)
            {
                validSpawnPoints.Add(spawnPoint);
            }
        }

        return validSpawnPoints.ToArray();
    }

    void CheckAndRemoveDistantEnemies()
    {
        List<GameObject> enemiesToRemove = new List<GameObject>(); // List to hold enemies that will be removed
        int enemiesToDestroy = 0;

        // Loop through active enemies to check their distance from the player
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                float distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);

                // Debug log to track the distance calculation
                //Debug.Log($"Enemy {enemy.name} is {distanceToPlayer} units from player.");

                // If the enemy is beyond the destruction range, mark it for removal
                if (distanceToPlayer > destructionRange)
                {
                    enemiesToDestroy++;
                    enemiesToRemove.Add(enemy); // Add the enemy to the removal list
                }
            }
        }

        // Debug log for how many enemies are being destroyed
        //Debug.Log($"Enemies to destroy: {enemiesToDestroy}");

        // Remove the enemies marked for destruction
        foreach (GameObject enemy in enemiesToRemove)
        {
            if (enemy != null)
            {
                Destroy(enemy);
                activeEnemies.Remove(enemy); // Remove from the active enemies list
                enemyCount--; // Decrease enemy count
                //Debug.Log($"Destroyed {enemy.name}");
            }
        }

    }

    // Draw the spawn range and active spawn points as Gizmos in the Scene view
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, spawnRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(player.position, minSpawnDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.position, destructionRange);
        }

        // Draw the spawn points
        if (m_SpawnPoints != null)
        {
            Gizmos.color = Color.yellow;
            foreach (Transform spawnPoint in m_SpawnPoints)
            {
                if (spawnPoint != null)
                {
                    Gizmos.DrawSphere(spawnPoint.position, 0.5f); // Adjust size as needed
                }
            }
        }
    }
}

