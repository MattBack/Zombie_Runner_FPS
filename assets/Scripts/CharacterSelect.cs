using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] GameObject playerOneSelectedChar;
    [SerializeField] GameObject playerTwoSelectedChar;
    [SerializeField] GameObject playerThreeSelectedChar;
    [SerializeField] GameObject playerFourSelectedChar;

    [SerializeField] Image playerOneCharacterImage;
    [SerializeField] Image playerTwoCharacterImage;
    [SerializeField] Image playerThreeCharacterImage;
    [SerializeField] Image playerFourCharacterImage;

    [SerializeField] Sprite characterDefaultImage;

    [SerializeField] Image CharacterOne;
    [SerializeField] Image CharacterTwo;
    [SerializeField] Image CharacterThree;
    [SerializeField] Image CharacterFour;
    [SerializeField] Image CharacterFive;
    [SerializeField] Image CharacterSix;
    [SerializeField] Image CharacterSeven;
    [SerializeField] Image CharacterEight;

    public bool characterIsSelected;

    public Color unSelectedColor;
    public Color characterSelectedColor;

    void Start()
    {   
        //TODO: Display UI depending on how many players were chosen

        // Set default character when the game starts
        playerOneCharacterImage.sprite = characterDefaultImage;
        playerTwoCharacterImage.sprite = characterDefaultImage;
        playerThreeCharacterImage.sprite = characterDefaultImage;
        playerFourCharacterImage.sprite = characterDefaultImage;

    }

    void SetPlayerOneCharacter(Image characterImage)
    {
        playerOneCharacterImage.sprite = characterImage.sprite;
        Debug.Log("Player One Character is set to: " + characterImage.sprite);
    }

    public void OnCharacterButtonHover(BaseEventData eventData)
    {
        // Your existing code for handling the hover
        GameObject hoveredObject = (eventData as PointerEventData)?.pointerEnter;

        if (hoveredObject != null && !characterIsSelected)
        {
            string buttonName = hoveredObject.name;

            // Use the button name to set the corresponding character image
            switch (buttonName)
            {
                case "CharacterOneButton":
                    SetPlayerOneCharacter(CharacterOne);
                    break;
                case "CharacterTwoButton":
                    SetPlayerOneCharacter(CharacterTwo);
                    break;
                case "CharacterThreeButton":
                    SetPlayerOneCharacter(CharacterThree);
                    break;
                case "CharacterFourButton":
                    SetPlayerOneCharacter(CharacterFour);
                    break;
                case "CharacterFiveButton":
                    SetPlayerOneCharacter(CharacterFive);
                    break;
                case "CharacterSixButton":
                    SetPlayerOneCharacter(CharacterSix);
                    break;
                case "CharacterSevenButton":
                    SetPlayerOneCharacter(CharacterSeven);
                    break;
                case "CharacterEightButton":
                    SetPlayerOneCharacter(CharacterEight);
                    break;
                //case "CharacterNineButton":
                //    SetPlayerOneCharacter(CharacterNine);
                //    break;
                    // Add cases for other buttons as needed
            }

            Debug.Log("Button is hovering: " + buttonName);
        }
    }

    public void CharacterLockedIn(BaseEventData eventData)
    {
        GameObject clickedObject = (eventData as PointerEventData)?.pointerPress;

        if (clickedObject != null)
        {
            Button clickedButton = clickedObject.GetComponent<Button>();

            if (clickedButton != null)
            {
                string testName = clickedObject.name;

                // Toggle character selection
                characterIsSelected = !characterIsSelected;
                
                Debug.Log("Character is " + (characterIsSelected ? "selected" : "no longer selected") + ". Clicked button name: " + testName);


                // Get the current colors
                ColorBlock colors = clickedButton.colors;
                
                //TODO update this for selecting a button
                colors.normalColor = characterIsSelected ? characterSelectedColor : unSelectedColor;
                //colors.selectedColor = characterIsSelected ? unSelectedColor : characterSelectedColor;

                // Apply the modified colors back to the button
                clickedButton.colors = colors;
                //Debug.Log(colors);

                // If character is locked in, set the player one character
                if (characterIsSelected)
                {
                    playerOneCharacterImage.sprite = CharacterOne.sprite;
                    Debug.Log("SetPlayerOneImage");
                } else
                {
                    Debug.Log("RemovePlayerOneImage");
                }
            }
        }
    }

    public void OnCharacterButtonHoverExit()
    {
        if (!characterIsSelected) {
            playerOneCharacterImage.sprite = characterDefaultImage;
        }
        
        // Handle hover exit if needed
        //Debug.Log("Mouse exited the button");
    }


}
