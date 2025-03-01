using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChosen : MonoBehaviour
{
    public CharacterInfo info;

    public Text nameText;

    public Image characterSelectedImage;

    public Text characterMaxHealth;

    void Start()
    {
        // TODO: what do we want to do on start?
        // maybe set to a default image/name etc
    }

    public void CharacterSelected() {
        // show name
        nameText.text = info.characterName;
        //show image
        characterSelectedImage.sprite = info.characterPic;
        // show max health
        characterMaxHealth.text = info.characterMaxHealth.ToString();
    }
}
