using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    #region Class Variables

    public CharacterInfo characterInfo;
    public CharacterStats characterStats;

    public CharacterInventory characterInventory;
    public WeaponTypeSet favoredWeaponTypes;

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

        favoredWeaponTypes = new WeaponTypeSet(templateArg.favoredWeaponTypesTemplate);

        characterInventory = new CharacterInventory(templateArg.characterInventoryTemplate);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        characterInfo = new CharacterInfo();
        characterStats = new CharacterStats();

        characterInventory = new CharacterInventory();
        favoredWeaponTypes = new WeaponTypeSet();
    }

    private void Setup(CharacterData templateArg)
    {
        characterInfo = new CharacterInfo(templateArg.characterInfo);
        characterStats = new CharacterStats(templateArg.characterStats);

        characterInventory = new CharacterInventory(templateArg.characterInventory);
        favoredWeaponTypes = new WeaponTypeSet(templateArg.favoredWeaponTypes);
    }

    #endregion


}
