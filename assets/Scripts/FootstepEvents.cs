using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FootstepSoundMapping
{
    public Texture2D texture;
    public AudioClip[] footstepSounds;
}

public class FootstepEvents : MonoBehaviour
{
    [SerializeField]
    public AudioClip[] stepClips;

    private AudioSource audioSource;

    private Terrain terrain;
    private TerrainData terrainData;

    public List<FootstepSoundMapping> footstepSounds = new List<FootstepSoundMapping>();
    public AudioClip defaultFootstepSound;

    private Vector3 lastPlayerPosition;
    private bool canPlayFootstep = true;
    private float footstepCooldown = 0.2f;
    private float currentCooldown = 0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        terrain = Terrain.activeTerrain;
        terrainData = terrain.terrainData;
        lastPlayerPosition = transform.position;
    }

    private void Update()
    {
        currentCooldown -= Time.deltaTime;

        Vector3 playerPosition = transform.position;
        if (playerPosition != lastPlayerPosition && currentCooldown <= 0f)
        {
            float terrainX = (playerPosition.x - terrain.transform.position.x) / terrainData.size.x;
            float terrainZ = (playerPosition.z - terrain.transform.position.z) / terrainData.size.z;
            int mapX = (int)(terrainX * terrainData.alphamapWidth);
            int mapZ = (int)(terrainZ * terrainData.alphamapHeight);
            int mapSize = terrainData.alphamapResolution;

            float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
            float[] textureValues = new float[terrainData.alphamapLayers];

            for (int i = 0; i < terrainData.alphamapLayers; i++)
            {
                textureValues[i] = splatmapData[0, 0, i];
            }

            int textureIndex = GetIndexOfHighestValue(textureValues);
            Texture2D texture = terrainData.terrainLayers[textureIndex].diffuseTexture;

            FootstepSoundMapping mapping = footstepSounds.Find(x => x.texture == texture);
            if (mapping != null)
            {
                AudioClip[] footstepSounds = mapping.footstepSounds;
                AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
                audioSource.clip = footstepSound;
            }
            else
            {
                audioSource.clip = defaultFootstepSound;
            }

            //audioSource.Play();

            currentCooldown = footstepCooldown;
        }

        lastPlayerPosition = playerPosition;
    }

    private int GetIndexOfHighestValue(float[] values)
    {
        int highestIndex = 0;
        float highestValue = 0f;

        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] > highestValue)
            {
                highestIndex = i;
                highestValue = values[i];
            }
        }

        return highestIndex;
    }

    private void StepWalk()
    {
        if (audioSource.clip == null)
        {
            audioSource.clip = defaultFootstepSound;
        }

        audioSource.PlayOneShot(audioSource.clip);
    }

    private AudioClip GetRandomClip()
    {
        return stepClips[Random.Range(0, stepClips.Length)];
    }
}
