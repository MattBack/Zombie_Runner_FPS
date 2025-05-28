using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;

    private List<AudioSource> audioSources = new List<AudioSource>();

    void Awake()
    {
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.bypassReverbZones = s.bypassReverbZones;
            s.source.reverbZoneMix = s.reverbZoneMix;
            s.source.spatialBlend = s.spatialBlend;
            s.source.loop = s.loop;

            audioSources.Add(s.source);
        }
    }

    public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!" + " - Please add sound in Audio Manager");
            return;
        }
        s.source.Play();
    }

    // Use OnDestroy to clean up the audio sources when the AudioManager is destroyed.
    void OnDestroy()
    {
        for (int i = audioSources.Count - 1; i >= 0; i--)
        {
            Destroy(audioSources[i]);
        }
    }
}
