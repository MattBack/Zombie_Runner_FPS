using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterInfo : ScriptableObject
{
    public string characterName;

    public int characterMaxHealth;

    public Sprite characterPic;

    public GameObject charcterPrefab;
}
