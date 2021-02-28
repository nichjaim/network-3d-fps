using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    #region Class Variables

    public CharacterInfo characterInfo;
    public CharacterStats characterStats;

    #endregion




    #region Constructors

    public CharacterData()
    {
        Setup();
    }

    public CharacterData(CharacterData templateArg)
    {
        Setup(templateArg);
    }

    public CharacterData(CharacterDataTemplate templateArg)
    {
        Setup(templateArg.template);

        // set color hex code from the template's character color
        characterInfo.characterHexColorCode = ColorUtility.ToHtmlStringRGB(templateArg.characterInfoColor);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        characterInfo = new CharacterInfo();
        characterStats = new CharacterStats();
    }

    private void Setup(CharacterData templateArg)
    {
        characterInfo = new CharacterInfo(templateArg.characterInfo);
        characterStats = new CharacterStats(templateArg.characterStats);
    }

    #endregion


}
