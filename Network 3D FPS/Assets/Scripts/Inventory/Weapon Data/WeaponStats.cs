using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    #region Class Variables

    public int damageBoundaryLowBase;
    public int damageBoundaryHighBase;

    [HideInInspector]
    public int damageBoundaryLowRarityModified;
    [HideInInspector]
    public int damageBoundaryHighRarityModified;

    public float attackRateBase;

    [HideInInspector]
    public float attackRateRarityModified;

    public int ammoPerUse;
    public float projectileSpeed;
    public float raycastRange;

    #endregion




    #region Constructors

    public WeaponStats()
    {
        Setup();
    }

    public WeaponStats(WeaponStats templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        damageBoundaryLowBase = 1;
        damageBoundaryHighBase = 1;

        damageBoundaryLowRarityModified = 1;
        damageBoundaryHighRarityModified = 1;

        attackRateBase = 1f;

        attackRateRarityModified = 1f;

        ammoPerUse = 1;
        projectileSpeed = 10f;
        raycastRange = 1f;
    }

    private void Setup(WeaponStats templateArg)
    {
        damageBoundaryLowBase = templateArg.damageBoundaryLowBase;
        damageBoundaryHighBase = templateArg.damageBoundaryHighBase;

        damageBoundaryLowRarityModified = templateArg.damageBoundaryLowRarityModified;
        damageBoundaryHighRarityModified = templateArg.damageBoundaryHighRarityModified;

        attackRateBase = templateArg.attackRateBase;

        attackRateRarityModified = templateArg.attackRateRarityModified;

        ammoPerUse = templateArg.ammoPerUse;
        projectileSpeed = templateArg.projectileSpeed;
        raycastRange = templateArg.raycastRange;
    }

    /*public void SetupRarityModifiers(RarityTier rarityTierArg)
    {
        //initalize rarity multipler var
        float randomRarityMultiplier;

        //get a random stat multiplier for the damage stat from the given rarity
        randomRarityMultiplier = Random.Range(rarityTierArg.statMultiplierBoundaryLow,
            rarityTierArg.statMultiplierBoundaryHigh);
        //set the rarity modified damage boundary stats using the retrieved modifier
        damageBoundaryLowRarityModified = Mathf.RoundToInt(damageBoundaryLowBase * randomRarityMultiplier);
        damageBoundaryHighRarityModified = Mathf.RoundToInt(damageBoundaryHighBase * randomRarityMultiplier);

        //if rarity modified damage LOW boundary is an invalid value
        if (damageBoundaryLowRarityModified < 0)
        {
            //set the rarity modified damage LOW boundary to the default valid value
            damageBoundaryLowRarityModified = 0;
        }
        //if rarity modified damage HIGH boundary is an invalid value
        if (damageBoundaryHighRarityModified < 0)
        {
            //set the rarity modified HIGH damage boundary to the default valid value
            damageBoundaryHighRarityModified = 0;
        }

        //get a random stat multiplier for the attack rate stat from the given rarity
        randomRarityMultiplier = Random.Range(rarityTierArg.statMultiplierBoundaryLow,
            rarityTierArg.statMultiplierBoundaryHigh);
        //set the rarity modified attack rate stat using the retrieved modifier
        attackRateRarityModified = attackRateBase / randomRarityMultiplier;

        //if rarity modified attack rate is an invalid value
        if (attackRateRarityModified < 0f)
        {
            //set the rarity modified attack rate to the default valid value
            attackRateRarityModified = 0f;
        }
    }*/

    #endregion


}
