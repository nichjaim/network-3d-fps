using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveAbility
{
    #region Class Variables

    [Header("Info Properties")]

    public string abilityId;
    //public string characterIdOfAssociatedCharacter;

    public string abilityName;
    [TextArea(3, 15)]
    public string abilityDescription;

    public int abilityRank;

    [Header("Resource Cost Properties")]

    public int resourceCostBase;
    public float resourceCostRankIncreasePercentage;
    public int resourceCostCeiling;

    [Header("Cooldown Time Properties")]

    public float cooldownTimeBase;
    public float cooldownTimeRankReductionPercentage;
    public float cooldownTimeFloor;

    #endregion




    #region Constructors

    public ActiveAbility()
    {
        Setup();
    }

    public ActiveAbility(ActiveAbility templateArg)
    {
        Setup(templateArg);
    }

    public ActiveAbility(ActiveAbilityTemplate templateArg)
    {
        Setup(templateArg.template);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        abilityId = "0";
        //characterIdOfAssociatedCharacter = "0";

        abilityName = "NAME";
        abilityDescription = "DESC";

        abilityRank = 1;

        resourceCostBase = 1;
        resourceCostRankIncreasePercentage = 0.1f;
        resourceCostCeiling = 1;

        cooldownTimeBase = 1f;
        cooldownTimeRankReductionPercentage = 0.1f;
        cooldownTimeFloor = 1f;
    }

    private void Setup(ActiveAbility templateArg)
    {
        abilityId = templateArg.abilityId;
        //characterIdOfAssociatedCharacter = templateArg.characterIdOfAssociatedCharacter;

        abilityName = templateArg.abilityName;
        abilityDescription = templateArg.abilityDescription;

        abilityRank = templateArg.abilityRank;

        resourceCostBase = templateArg.resourceCostBase;
        resourceCostRankIncreasePercentage = templateArg.resourceCostRankIncreasePercentage;
        resourceCostCeiling = templateArg.resourceCostCeiling;

        cooldownTimeBase = templateArg.cooldownTimeBase;
        cooldownTimeRankReductionPercentage = templateArg.cooldownTimeRankReductionPercentage;
        cooldownTimeFloor = templateArg.cooldownTimeFloor;
    }

    #endregion




    #region Active Ability Functions

    /// <summary>
    /// Returns the active ability's resource cost with ability rank factored in.
    /// </summary>
    /// <returns></returns>
    public int GetTrueActiveAbilityResourceCost()
    {
        // if the ability is at rank one
        if (abilityRank <= 1)
        {
            // return the base cost
            return resourceCostBase;
        }

        /// get the precentage to increase based on the rank
        /// NOTE: need to use rank - 1 because the increases only start after the first rank
        float resourceCostRankIncrease = resourceCostRankIncreasePercentage * (abilityRank - 1);
        // get the amount to increase the cost by
        float costIncrease = resourceCostBase * resourceCostRankIncrease;

        // get the actual resource cost as a float
        float trueResourceCostFloat = resourceCostBase + costIncrease;
        // round the actual resource cost to an int
        int trueResourceCostInt = Mathf.RoundToInt(trueResourceCostFloat);

        // if the retrieved cost is higher then the cost's ceiling boundary
        if (trueResourceCostInt > resourceCostCeiling)
        {
            //set the actual cost back down to the cost's ceiling boundary
            trueResourceCostInt = resourceCostCeiling;
        }

        // if the retrieved cost is below ZERO
        if (trueResourceCostInt < 0)
        {
            // set the actual cost back up to ZERO
            trueResourceCostInt = 0;
        }

        // return the retrieved true cost
        return trueResourceCostInt;
    }

    /// <summary>
    /// Returns the active ability's cooldown time with ability rank factored in.
    /// </summary>
    /// <returns></returns>
    public float GetTrueActiveAbilityCooldownTime()
    {
        // if the ability is at rank one
        if (abilityRank <= 1)
        {
            // return the base time
            return cooldownTimeBase;
        }

        /// get the precentage to reduce based on the rank
        /// NOTE: need to use rank - 1 because the reductions only start after the first rank
        float cooldownTimeRankReduction = cooldownTimeRankReductionPercentage * (abilityRank - 1);
        // get the amount to reduce the time by
        float timeReduction = cooldownTimeBase * cooldownTimeRankReduction;

        // get the actual coooldown time
        float trueCooldownTime = cooldownTimeBase - timeReduction;

        // if the retrieved time is lower then the time's floor boundary
        if (trueCooldownTime < cooldownTimeFloor)
        {
            // set the actual time back up to the time's floor boundary
            trueCooldownTime = cooldownTimeFloor;
        }

        // if the retrieved time is below ZERO
        if (trueCooldownTime < 0)
        {
            // set the actual time back up to ZERO
            trueCooldownTime = 0;
        }

        // return the retrieved true time
        return trueCooldownTime;
    }

    /// <summary>
    /// Increases the rank of the active ability.
    /// </summary>
    public void PromoteAbilityRank()
    {
        // increment rank
        abilityRank++;
    }

    #endregion


}
