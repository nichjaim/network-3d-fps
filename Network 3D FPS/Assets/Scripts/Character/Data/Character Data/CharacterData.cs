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

    public FactionReputation factionReputation;

    public ActiveAbilityLoadout activeAbilityLoadout;

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

        factionReputation = new FactionReputation(templateArg.factionReputationTemplate);

        activeAbilityLoadout = new ActiveAbilityLoadout();
        foreach (ActiveAbilityTemplate iterAbilityTemp in templateArg.activeAbilityTemplates)
        {
            activeAbilityLoadout.AddActiveAbility(new ActiveAbility(iterAbilityTemp));
        }
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        characterInfo = new CharacterInfo();
        characterStats = new CharacterStats();

        characterInventory = new CharacterInventory();
        favoredWeaponTypes = new WeaponTypeSet();

        factionReputation = new FactionReputation();

        activeAbilityLoadout = new ActiveAbilityLoadout();
    }

    private void Setup(CharacterData templateArg)
    {
        characterInfo = new CharacterInfo(templateArg.characterInfo);
        characterStats = new CharacterStats(templateArg.characterStats);

        characterInventory = new CharacterInventory(templateArg.characterInventory);
        favoredWeaponTypes = new WeaponTypeSet(templateArg.favoredWeaponTypes);

        factionReputation = new FactionReputation(templateArg.factionReputation);

        activeAbilityLoadout = new ActiveAbilityLoadout(templateArg.activeAbilityLoadout);
    }

    #endregion


}
