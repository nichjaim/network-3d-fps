using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInfo
{
    #region Class Variables

    public string characterId;
    public string characterName;
    public float characterHeightInCm;

    [HideInInspector]
    public string characterHexColorCode;

    #endregion




    #region Constructor Functions

    public CharacterInfo()
    {
        Setup();
    }

    public CharacterInfo(CharacterInfo templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        characterId = "0";
        characterName = "NAME";
        // below-average height (5'5)
        characterHeightInCm = 165f;

        characterHexColorCode = "FFFFFF";
    }

    private void Setup(CharacterInfo templateArg)
    {
        characterId = templateArg.characterId;
        characterName = templateArg.characterName;
        characterHeightInCm = templateArg.characterHeightInCm;

        characterHexColorCode = templateArg.characterHexColorCode;
    }

    #endregion


}
