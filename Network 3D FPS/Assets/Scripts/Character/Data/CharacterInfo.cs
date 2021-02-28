using UnityEngine;

[System.Serializable]
public class CharacterInfo
{
    #region Class Variables

    public string characterId;
    public string characterName;
    public float characterHeight;

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
        characterHeight = 1f;

        characterHexColorCode = "FFFFFF";
    }

    private void Setup(CharacterInfo templateArg)
    {
        characterId = templateArg.characterId;
        characterName = templateArg.characterName;
        characterHeight = templateArg.characterHeight;

        characterHexColorCode = templateArg.characterHexColorCode;
    }

    #endregion


}
