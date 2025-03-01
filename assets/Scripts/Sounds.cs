using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]

public class Sounds {
    public AudioClip clip;

    public string name;

    [Range(0.1f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool bypassReverbZones;
    [Range(0.1f, 1f)]
    public float reverbZoneMix;
    [Range(0f, 1f)]
    public float spatialBlend;
    public bool loop;

    // variable is public but won't show in inspector
    [HideInInspector]
    public AudioSource source;
}

