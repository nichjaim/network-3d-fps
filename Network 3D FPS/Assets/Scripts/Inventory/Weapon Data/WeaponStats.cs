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

    public int projectilesPerShot;
    public float shotSpread;

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

        projectilesPerShot = 1;
        shotSpread = 0f;
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

        projectilesPerShot = templateArg.projectilesPerShot;
        shotSpread = templateArg.shotSpread;
    }

    /// <summary>
    /// Sets the rarity modified values based on the given rarity.
    /// </summary>
    /// <param name="rarityTierArg"></param>
    public void SetupRarityModifiers(RarityTier rarityTierArg)
    {
        // get a random stat multiplier for the damage stat from the given rarity
        /*float randomRarityMultiplier = Random.Range(rarityTierArg.statMultiplierBoundaryLow,
            rarityTierArg.statMultiplierBoundaryHigh);*/
        // TEMP LINE!!! Apparently, Random cannot be called during serilization so just doing this for now...
        float randomRarityMultiplier = (rarityTierArg.statMultiplierBoundaryLow + rarityTierArg.statMultiplierBoundaryHigh) / 2f;

        // set the rarity modified damage boundary stats using the retrieved modifier
        damageBoundaryLowRarityModified = Mathf.RoundToInt(damageBoundaryLowBase * randomRarityMultiplier);
        damageBoundaryHighRarityModified = Mathf.RoundToInt(damageBoundaryHighBase * randomRarityMultiplier);

        // ensure rarity modified damage boundaries are valid values
        damageBoundaryLowRarityModified = Mathf.Max(damageBoundaryLowRarityModified, 0);
        damageBoundaryHighRarityModified = Mathf.Max(damageBoundaryHighRarityModified, 0);

        // get a random stat multiplier for the attack rate stat from the given rarity
        /*randomRarityMultiplier = Random.Range(rarityTierArg.statMultiplierBoundaryLow,
            rarityTierArg.statMultiplierBoundaryHigh);*/
        // TEMP LINE!!! Apparently, Random cannot be called during serilization so just doing this for now...
        randomRarityMultiplier = (rarityTierArg.statMultiplierBoundaryLow + rarityTierArg.statMultiplierBoundaryHigh) / 2f;

        // set the rarity modified attack rate stat using the retrieved modifier
        attackRateRarityModified = attackRateBase / randomRarityMultiplier;

        // ensure rarity modified attack rate is a valid value
        attackRateRarityModified = Mathf.Max(attackRateRarityModified, 0f);
    }

    #endregion


}
