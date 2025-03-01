using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DamageNumbersPro;


public class GameEventsManager : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI startText;

    public DamageNumber numberPrefab; // reference to the damage number prefab to be displayed when enemy takes damage

    #region kill info  
    public int KillCount = 0;

    [SerializeField] TextMeshProUGUI killCountText;
    #endregion

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        Invoke("PlayStartSound1", 2f);
        Invoke("PlayStartSound2", 4f);
        //Invoke("startText", 20f);
        Destroy(startText, 3f);
    }

    private void Update()
    {
        DisplayKillCount();
    }

    private void DisplayKillCount()
    {
        int currentKillCount = KillCount;
        killCountText.text = currentKillCount.ToString();
    }

    void PlayStartSound1() { 
        audioManager.Play("GameStartSfx");
    }

    void PlayStartSound2()
    {
        audioManager.Play("GameStartLaugh");
    }




}
