using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterElementHandler : MonoBehaviour
{
    [SerializeField] ChooseCharacter[] chooseCharacterElements;
    // Start is called before the first frame update
    void Start()
    {
        chooseCharacterElements = GetComponentsInChildren<ChooseCharacter>();
    }

    public void CleanSelection(CharacterType characterType, string name)
    {
        foreach (ChooseCharacter element in chooseCharacterElements)
        {

            if (characterType != element.characterType && element.playerName == name && element.playerName != "")
            {
                element.ResetData();
            }
        }
    }
}
