using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechShotAbilityController : ActiveAbilityProjectileController
{
    #region Class Variables

    // how long the inflicted debuff will last
    private float effectDuration = 1f;
    // how much of the damage done to debuff target will heal attacker
    private float damageHealingPercentage = 0.1f;

    private LeechDamageStatusEffect leechStatusEffect = null;

    #endregion




    #region Override Functions

    public override void SetupActiveAbility(ActiveAbility castedActiveAbilityArg,
        CharacterMasterController casterCharacterArg, CharacterData casterCharacterDataArg)
    {
        base.SetupActiveAbility(castedActiveAbilityArg, casterCharacterArg, casterCharacterDataArg);

        // setup ability proeprties
        SetupAbilityDamageHealingPercentage();
        SetupAbilityEffectDuration();

        // setup the actual status effect ability data
        SetupAbilityStatusEffect(damageHealingPercentage);
    }

    protected override void OnContactWithEnemy(HitboxController enemyHitboxArg)
    {
        base.OnContactWithEnemy(enemyHitboxArg);

        // apply weakening debuff to hit target
        enemyHitboxArg.CharMaster.CharStatus.AddStatusEffect(leechStatusEffect, effectDuration);

        // increment number of targets penetrated and despawns if reached penetration limit
        ProcessTargetPenetration();
    }

    protected override void SetupAbilityTargetPenetrationMax()
    {
        // initialize ability damage taken property values
        int baseValue = 1;
        float rankAdditionValue = 0.1f;
        int maxValue = 100;

        // calculate the value (every 10th level should increase max pent by one)
        int calcValue = Mathf.FloorToInt((float)baseValue + (rankAdditionValue * 
            (float)(_appropriateAbilityRank - 1)));

        // ensure the calcualted value is within valid parameters
        calcValue = Mathf.Clamp(calcValue, baseValue, maxValue);

        // set max pentration to calcualted value
        targetPenetrationMax = calcValue;
    }

    #endregion




    #region Setup Functions

    /// <summary>
    /// Adjusts the ability's damage healing percentage based on the given ability rank argument.
    /// </summary>
    private void SetupAbilityDamageHealingPercentage()
    {
        // initialize ability damage taken property values
        float baseValue = 0.15f;
        float rankAdditionValue = 0.05f;
        float maxValue = 10f;

        // get new calculated percentage value
        float newDmgPerc = Mathf.Clamp(baseValue + (rankAdditionValue *
            (_appropriateAbilityRank - 1)), baseValue, maxValue);

        // set damage healing percentage to calcualted value
        damageHealingPercentage = newDmgPerc;
    }

    /// <summary>
    /// Adjusts the ability's status effect duration based on the given ability rank argument.
    /// </summary>
    private void SetupAbilityEffectDuration()
    {
        // initialize ability effect duration property values
        float baseValue = 6f;
        float rankAdditionValue = 1f;
        float maxValue = 300f;

        // get new calculated percentage value
        float newEffectDur = Mathf.Clamp(baseValue + (rankAdditionValue *
            (_appropriateAbilityRank - 1)), baseValue, maxValue);

        // set status effect duration to calcualted value
        effectDuration = newEffectDur;
    }

    /// <summary>
    /// Sets up the actual status effect ability data.
    /// </summary>
    /// <param name="dmgHealPercArg"></param>
    private void SetupAbilityStatusEffect(float dmgHealPercArg)
    {
        leechStatusEffect = new LeechDamageStatusEffect(
            _casterCharData.factionReputation, dmgHealPercArg);
    }

    #endregion


}
