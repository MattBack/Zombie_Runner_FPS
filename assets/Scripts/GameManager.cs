using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Characters
    public NumberOfPlayers numberOfPlayers = NumberOfPlayers.One;

    public static List<CharacterInfo> SelectedCharacters = new List<CharacterInfo>();

    // Levels
    [SerializeField]
    public LevelSelector levelSelector = LevelSelector.LevelOne;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
                    
    }

    // use the on click event of each button to say how many players there are.

    public void SetOnePlayer() {
        numberOfPlayers = NumberOfPlayers.One;
        //Debug.Log("Number of players is: " + (int)numberOfPlayers);
    }

    public void SetTwoPlayer()
    {
        numberOfPlayers = NumberOfPlayers.Two;
       // Debug.Log("Number of players is: " + (int)numberOfPlayers);
    }

    public void SetThreePlayer()
    {
        numberOfPlayers = NumberOfPlayers.Three;
        //Debug.Log("Number of players is: " + (int)numberOfPlayers);
    }

    public void SetFourPlayer()
    {
        numberOfPlayers = NumberOfPlayers.Four;
        //Debug.Log("Number of players is: " + (int)numberOfPlayers);
    }

    // Level select functions

    public void SetLevelOne()
    {
        levelSelector = LevelSelector.LevelOne;
        Debug.Log("Current Level is: " + levelSelector);
    }

    public void SetLevelTwo()
    {
        levelSelector = LevelSelector.LevelTwo;
        Debug.Log("Current Level is: " + levelSelector);
    }

    public void SetLevelThree()
    {
        levelSelector = LevelSelector.LevelThree;
        Debug.Log("Current Level is: " + levelSelector);
    }

    public void SetLevelFour()
    {
        levelSelector = LevelSelector.LevelFour;
        Debug.Log("Current Level is: " + levelSelector);
    }


}
