using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private NumberOfPlayers numberOfPlayers = NumberOfPlayers.One;

    [SerializeField]
    public LevelSelector levelSelector = LevelSelector.LevelOne;   
 
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
            
    }

    // use the on click event of each button to say how many players there are.

    public void SetOnePlayer() {
        numberOfPlayers = NumberOfPlayers.One;
    }

    public void SetTwoPlayer()
    {
        numberOfPlayers = NumberOfPlayers.Two;
    }

    public void SetThreePlayer()
    {
        numberOfPlayers = NumberOfPlayers.Three;
    }

    public void SetFourPlayer()
    {
        numberOfPlayers = NumberOfPlayers.Four;
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
