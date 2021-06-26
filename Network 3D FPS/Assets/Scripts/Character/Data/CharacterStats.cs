using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    #region Class Variables

    [Header("Combat Resources")]

    public int healthMaxBase;
    [HideInInspector]
    public int healthCurrent;
    // the amount of max health that is inaccessible due to exhaustion
    [HideInInspector]
    public int healthFatigue;
    // the amount of points built up which at a certaoin point will be converted to a health fatigue point
    [HideInInspector]
    public int healthFatigueBuildup;

    // nicknamed energy, lets you perform skills
    public int manaMaxBase;
    [HideInInspector]
    public int manaCurrent;
    // the amount of max mana that is inaccessible due to exhaustion
    [HideInInspector]
    public int manaFatigue;
    // the amount of points built up which at a certaoin point will be converted to a mana fatigue point
    [HideInInspector]
    public int manaFatigueBuildup;

    // the cap amount that dictates at what point fatigue buildup gets covnerted to a fatgiue point
    public int fatigueBuildupCap;
    /// the max percentage of fatigue that can be built up on a combat resource. 
    /// (ex. if at 0.7 then health and mana fatigue can't cover more than 70% of the total amount)
    public float fatigueMaxPercentage;

    [Header("Primary Stats")]

    // no nickname, increases max health
    public int statStamina;
    // nicknamed wits, increases max energy
    public int statIntelligence;
    // no nickname, increases melee attack damage
    public int statStrength;
    // nicknamed precision, increases ranged attack damage
    public int statDexterity;

    [Header("Movement Stats")]

    public float statMovementSpeed;
    public float statJumpHeight;
    public int statConsecutiveJumps;

    [Header("Misc Stats")]

    // no nickname, reduces damage taken (exclusively for tank character)
    public int statDefense;
    // no nickname, increases healing done (exclusively for healer character)
    public int statHealing;
    // multiplies threat of actions done, whether increasing it or reducing it
    public float threatMultiplier;

    // the amount of time after taking damage that the character cannot take damage again
    public float postDamageInvincibilityTime;

    // the amount of time it takes for a mana regen instance to proc
    public float manaRegenerationRate;
    // the amount of mana regenerated when mana regen procs
    public int manaRegenerationAmount;

    /// The level improvement to all active abilities. (Ex. specific ability at 
    /// level 4 and this stat at 3 then that ability would be consiered level 6 
    /// as first level of this stat doesn't really count as it's the default.
    public int statAllAbilityProficiencyLevel;

    [Header("Utility Stats")]

    //the size of the flashlight lighting sources
    public float flashlightReachDirectional;
    public float flashlightReachOmniDirectional;

    #endregion




    #region Constructors

    public CharacterStats()
    {
        Setup();
    }

    public CharacterStats(CharacterStats templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        healthMaxBase = 1;
        healthCurrent = 1;
        healthFatigue = 0;
        healthFatigueBuildup = 0;

        manaMaxBase = 1;
        manaCurrent = 1;
        manaFatigue = 0;
        manaFatigueBuildup = 0;

        fatigueBuildupCap = 10;
        fatigueMaxPercentage = 0.7f;

        statStamina = 1;
        statIntelligence = 1;
        statStrength = 1;
        statDexterity = 1;

        statMovementSpeed = 1f;
        statJumpHeight = 1f;
        statConsecutiveJumps = 1;

        statDefense = 0;
        statHealing = 0;
        threatMultiplier = 1f;

        postDamageInvincibilityTime = 0f;

        manaRegenerationRate = 1f;
        manaRegenerationAmount = 1;

        statAllAbilityProficiencyLevel = 1;

        flashlightReachDirectional = 1f;
        flashlightReachOmniDirectional = 0f;
    }

    private void Setup(CharacterStats templateArg)
    {
        healthMaxBase = templateArg.healthMaxBase;
        /// set current health to the template's max health rather 
        /// than current as current health is innacessible to the inspector
        healthCurrent = templateArg.healthMaxBase;
        healthFatigue = templateArg.healthFatigue;
        healthFatigueBuildup = templateArg.healthFatigueBuildup;

        manaMaxBase = templateArg.manaMaxBase;
        /// set current mana to the template's max health rather 
        /// than current as current mana is innacessible to the inspector
        manaCurrent = templateArg.manaMaxBase;
        manaFatigue = templateArg.manaFatigue;
        manaFatigueBuildup = templateArg.manaFatigueBuildup;

        fatigueBuildupCap = templateArg.fatigueBuildupCap;
        fatigueMaxPercentage = templateArg.fatigueMaxPercentage;

        statStamina = templateArg.statStamina;
        statIntelligence = templateArg.statIntelligence;
        statStrength = templateArg.statStrength;
        statDexterity = templateArg.statDexterity;

        statMovementSpeed = templateArg.statMovementSpeed;
        statJumpHeight = templateArg.statJumpHeight;
        statConsecutiveJumps = templateArg.statConsecutiveJumps;

        statDefense = templateArg.statDefense;
        statHealing = templateArg.statHealing;
        threatMultiplier = templateArg.threatMultiplier;

        postDamageInvincibilityTime = templateArg.postDamageInvincibilityTime;

        manaRegenerationRate = templateArg.manaRegenerationRate;
        manaRegenerationAmount = templateArg.manaRegenerationAmount;

        statAllAbilityProficiencyLevel = templateArg.statAllAbilityProficiencyLevel;

        flashlightReachDirectional = templateArg.flashlightReachDirectional;
        flashlightReachOmniDirectional = templateArg.flashlightReachOmniDirectional;
    }

    #endregion




    #region Health Functions

    /// <summary>
    /// Returns the max health with any relevant modifiers applied.
    /// </summary>
    public int GetTrueMaxHealth()
    {
        return healthMaxBase + statStamina - healthFatigue;
    }

    /// <summary>
    /// Returns the max health with any relevant modifiers applied EXCEPT fatigue.
    /// </summary>
    /// <returns></returns>
    public int GetTrueMaxHealthWithoutFatigueModifier()
    {
        return healthMaxBase + statStamina;
    }

    /// <summary>
    /// Returns the percentage of health remaining. 
    /// NOTE: Need to cast both as a float before dividing to avoid integer divison.
    /// </summary>
    /// <returns></returns>
    public float GetCurrentHealthPercentage()
    {
        return (float)healthCurrent / (float)GetTrueMaxHealthWithoutFatigueModifier();
    }

    /// <summary>
    /// Returns the percentage of health that is locked due to fatigue. 
    /// NOTE: Need to cast both as a float before dividing to avoid integer divison.
    /// </summary>
    /// <returns></returns>
    public float GetHealthFatiguePercentage()
    {
        return (float)healthFatigue / (float)GetTrueMaxHealthWithoutFatigueModifier();
    }

    /// <summary>
    /// Sets the char's current health based on the given percentage.
    /// </summary>
    /// <param name="percentageArg"></param>
    public void SetHealthToPercentage(float percentageArg)
    {
        // if the char is still alive
        if (!IsOutOfHealth())
        {
            // get the new health value from the given percentage as a float
            float newHealthFloat = (float)GetTrueMaxHealthWithoutFatigueModifier() * percentageArg;
            // round ehalth value to an int
            int newHealth = Mathf.RoundToInt(newHealthFloat);
            // ensure new health value stays within the alive value boundaries
            newHealth = Mathf.Clamp(newHealth, 1, GetTrueMaxHealthWithoutFatigueModifier());

            // set current health to new calculated health
            healthCurrent = newHealth;
        }
        // else the char is already dead
        else
        {
            // set health to zero
            healthCurrent = 0;
        }
    }

    /// <summary>
    /// Decreases health based on given damage.
    /// </summary>
    /// <param name="damageArg"></param>
    public int TakeDamage(int damageArg)
    {
        // calcualte the actual damage inflicted
        int trueDamage = Mathf.Max(damageArg - statDefense, 0);

        // decrease current health by actual damage, potentially down to the health floor boundary
        healthCurrent = Mathf.Max(healthCurrent - trueDamage, 0);

        // builds up fatigue in health if apporpriate
        BuildFatigueHealth(damageArg);

        // return amount of actual damage taken
        return trueDamage;
    }

    /// <summary>
    /// Builds up fatigue in health if apporpriate.
    /// </summary>
    /// <param name="damageArg"></param>
    private void BuildFatigueHealth(int damageArg)
    {
        // increase the health fatigue buildup by the damage received
        healthFatigueBuildup += damageArg;

        // get the maximum possible health fatigue as a float
        float maxPossibleHealthFatigueFloat = GetTrueMaxHealthWithoutFatigueModifier() * fatigueMaxPercentage;
        // round the maximum possible health fatigue to an integer
        int maxPossibleHealthFatigueInt = Mathf.RoundToInt(maxPossibleHealthFatigueFloat);
        // while the health fatigue buildup amount merits conversion to health fatigue points
        while (healthFatigueBuildup >= fatigueBuildupCap)
        {
            // reduce fatigue buildup by the fatigue buildup cap
            healthFatigueBuildup -= fatigueBuildupCap;
            // increase health fatigue by one
            healthFatigue++;

            // if the health fatigue exceeds the max health fatigue that should be possible
            if (healthFatigue > maxPossibleHealthFatigueInt)
            {
                // set the health fatigue back down to the the max health fatigue possible
                healthFatigue = maxPossibleHealthFatigueInt;
            }
        }
    }

    /// <summary>
    /// Refills current health to max.
    /// </summary>
    public void FillHealthToMax()
    {
        healthCurrent = GetTrueMaxHealth();
    }

    /// <summary>
    /// Increases current health based on given heal.
    /// </summary>
    /// <param name="healArg"></param>
    public void HealHealth(int healArg)
    {
        // if health is NOT empty
        if (!IsOutOfHealth())
        {
            // increase current health by heal, potentially up to the health ceiling boundary
            healthCurrent = Mathf.Min(healthCurrent + healArg, GetTrueMaxHealth());
        }
    }

    /// <summary>
    /// Removes all fatigue on health.
    /// </summary>
    public void RemoveAllHealthFatigue()
    {
        healthFatigue = 0;
        healthFatigueBuildup = 0;
    }

    /// <summary>
    /// Brings current health back up to one if at zero.
    /// </summary>
    public void ResuscitateIfCriticallyHurt()
    {
        // if current health at zero
        if (IsOutOfHealth())
        {
            // bring current health back up to one.
            healthCurrent = 1;
        }
    }

    /// <summary>
    /// Returns bool that denotes if health is empty.
    /// </summary>
    /// <returns></returns>
    public bool IsOutOfHealth()
    {
        return healthCurrent <= 0;
    }

    #endregion




    #region Mana Functions

    /// <summary>
    /// Returns the max mana with any relevant modifiers applied.
    /// </summary>
    public int GetTrueMaxMana()
    {
        return manaMaxBase + statIntelligence - manaFatigue;
    }

    /// <summary>
    /// Returns the max mana with any relevant modifiers applied EXCEPT fatigue.
    /// </summary>
    /// <returns></returns>
    public int GetTrueMaxManaWithoutFatigueModifier()
    {
        return manaMaxBase + statIntelligence;
    }

    /// <summary>
    /// Returns the percentage of mana remaining. 
    /// NOTE: Need to cast both as a float before dividing to avoid integer divison.
    /// </summary>
    /// <returns></returns>
    public float GetCurrentManaPercentage()
    {
        return (float)manaCurrent / (float)GetTrueMaxManaWithoutFatigueModifier();
    }

    /// <summary>
    /// Returns the percentage of mana that is locked due to fatigue. 
    /// NOTE: Need to cast both as a float before dividing to avoid integer divison.
    /// </summary>
    /// <returns></returns>
    public float GetManaFatiguePercentage()
    {
        return (float)manaFatigue / (float)GetTrueMaxManaWithoutFatigueModifier();
    }

    /// <summary>
    /// Increases current mana based on given mana amount.
    /// </summary>
    /// <param name="manaAmountArg"></param>
    public void RecoverMana(int manaAmountArg)
    {
        // increase current mana by recover amount, potentially up to the mana ceiling boundary
        manaCurrent = Mathf.Min(manaCurrent + manaAmountArg, GetTrueMaxMana());
    }

    /// <summary>
    /// Recover mana based on regeneration amount.
    /// </summary>
    public void RegenerateMana()
    {
        RecoverMana(manaRegenerationAmount);
    }

    /// <summary>
    /// Returns bool that denotes if curret mana is at max capacity.
    /// </summary>
    /// <returns></returns>
    public bool IsManaFull()
    {
        return manaCurrent >= GetTrueMaxMana();
    }

    /// <summary>
    /// Decreases mana based on given cost.
    /// </summary>
    /// <param name="manaCostArg"></param>
    public void ReduceMana(int manaCostArg)
    {
        // decrease current mana by cost, potentially down to the mana floor boundary
        manaCurrent = Mathf.Max(manaCurrent - manaCostArg, 0);

        // builds up fatigue in mana if apporpriate
        BuildFatigueMana(manaCostArg);
    }

    /// <summary>
    /// Builds up fatigue in mana if apporpriate.
    /// </summary>
    /// <param name="manaCostArg"></param>
    private void BuildFatigueMana(int manaCostArg)
    {
        // increase the mana fatigue buildup by the cost received
        manaFatigueBuildup += manaCostArg;

        // get the maximum possible mana fatigue as a float
        float maxPossibleManaFatigueFloat = GetTrueMaxManaWithoutFatigueModifier() * fatigueMaxPercentage;
        // round the maximum possible mana fatigue to an integer
        int maxPossibleManaFatigueInt = Mathf.RoundToInt(maxPossibleManaFatigueFloat);

        // while the mana fatigue buildup amount merits conversion to mana fatigue points
        while (manaFatigueBuildup >= fatigueBuildupCap)
        {
            // reduce fatigue buildup by the fatigue buildup cap
            manaFatigueBuildup -= fatigueBuildupCap;
            // increase mana fatigue by one, potentially up to the mana fatigue ceiling boundary
            manaFatigue = Mathf.Min(manaFatigue + 1, maxPossibleManaFatigueInt);
        }
    }

    /// <summary>
    /// Refills current mana to max.
    /// </summary>
    public void FillManaToMax()
    {
        manaCurrent = GetTrueMaxMana();
    }

    /// <summary>
    /// Removes all fatigue on mana.
    /// </summary>
    public void RemoveAllManaFatigue()
    {
        manaFatigue = 0;
        manaFatigueBuildup = 0;
    }

    /// <summary>
    /// Returns bool that denotes if the character has enough mana to meet the 
    /// given mana cost.
    /// </summary>
    /// <param name="manaCostArg"></param>
    /// <returns></returns>
    public bool HaveEnoughMana(int manaCostArg)
    {
        return manaCurrent >= manaCostArg;
    }

    #endregion




    #region Stat Functions

    /// <summary>
    /// Improves character's stats/properties based on given progress values.
    /// </summary>
    /// <param name="progTypeArg"></param>
    /// <param name="progValueArg"></param>
    private void AddCharacterProgression(CharacterProgressionType progTypeArg, float progValueArg)
    {
        // round value to int for cases that require an integer
        int progInt = Mathf.RoundToInt(progValueArg);

        switch (progTypeArg)
        {
            case CharacterProgressionType.Stamina:
                statStamina += progInt;
                break;

            default:
                // print warning to console
                Debug.LogWarning($"Unknown case for AddCharacterProgression(): {progTypeArg.ToString()}");
                break;
        }
    }

    #endregion


}

