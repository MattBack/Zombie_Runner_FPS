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

    public Transform player; 
    public float spawnRange = 30f;
    public float minSpawnDistance = 10f;
    public float spawnDelay = 2f;
    public float checkInterval = 5f;
    public float destructionRange = 50f;
    public int minEnemiesNearPlayer = 3;

    private Transform[] m_SpawnPoints;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private int enemyPrefabCount;
    private bool isSpawning = false;

    public List<Transform> validSpawnPointsInInspector = new List<Transform>();

    void Start()
    {
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

        InvokeRepeating("CheckAndRemoveDistantEnemies", checkInterval, checkInterval);
    }

    void Update()
    {

        if (enemyCount < enemySpawnCount && !isSpawning)
        {
            StartCoroutine(SpawnEnemiesWithDelay());
        }

        UpdateValidSpawnPoints();
    }

    void UpdateValidSpawnPoints()
    {
        validSpawnPointsInInspector.Clear();

        foreach (Transform spawnPoint in m_SpawnPoints)
        {
            if (spawnPoint != null)
            {
                float distanceToPlayer = Vector3.Distance(player.position, spawnPoint.position);

                if (distanceToPlayer <= spawnRange && !IsPointDirectlyInFrontOfPlayer(spawnPoint.position))
                {
                    validSpawnPointsInInspector.Add(spawnPoint);
                }
            }
        }
    }

    IEnumerator SpawnEnemiesWithDelay()
    {
        isSpawning = true;

        while (enemyCount < enemySpawnCount)
        {        
            if (validSpawnPointsInInspector.Count > 0)
            {
                int randomSpawnPointNumber = Random.Range(0, validSpawnPointsInInspector.Count);
                int randomEnemyPrefabNumber = Random.Range(0, m_EnemyPrefabs.Length);

                GameObject newEnemy = Instantiate(m_EnemyPrefabs[randomEnemyPrefabNumber],
                                                   validSpawnPointsInInspector[randomSpawnPointNumber].position,
                                                   Quaternion.identity);

                activeEnemies.Add(newEnemy);

                enemyCount++;
                //Debug.Log($"Spawned new enemy: {newEnemy.name} at {newEnemy.transform.position}");
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
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

        if (validSpawnPoints.Length > 1)
        {
            int randomSpawnPointNumber = Random.Range(1, validSpawnPoints.Length);
            int randomEnemyPrefabNumber = Random.Range(0, enemyPrefabCount);

            GameObject newEnemy = Instantiate(m_EnemyPrefabs[randomEnemyPrefabNumber],
                         validSpawnPoints[randomSpawnPointNumber].position,
                         Quaternion.identity);

            activeEnemies.Add(newEnemy);
            enemyCount++;

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
        List<GameObject> enemiesToRemove = new List<GameObject>();
        int enemiesToDestroy = 0;

        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                float distanceToPlayer = Vector3.Distance(player.position, enemy.transform.position);

                // Debug log to track the distance calculation
                //Debug.Log($"Enemy {enemy.name} is {distanceToPlayer} units from player.");

                if (distanceToPlayer > destructionRange)
                {
                    enemiesToDestroy++;
                    enemiesToRemove.Add(enemy);
                }
            }
        }

        //Debug.Log($"Enemies to destroy: {enemiesToDestroy}");

        foreach (GameObject enemy in enemiesToRemove)
        {
            if (enemy != null)
            {
                Destroy(enemy);
                activeEnemies.Remove(enemy); 
                enemyCount--;
                //Debug.Log($"Destroyed {enemy.name}");
            }
        }

    }

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

        if (m_SpawnPoints != null)
        {
            Gizmos.color = Color.yellow;
            foreach (Transform spawnPoint in m_SpawnPoints)
            {
                if (spawnPoint != null)
                {
                    Gizmos.DrawSphere(spawnPoint.position, 0.5f); 
                }
            }
        }
    }
}

