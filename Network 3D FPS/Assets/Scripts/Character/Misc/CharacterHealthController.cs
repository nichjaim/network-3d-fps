using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private CharacterMasterController _characterMasterController = null;

    [SerializeField]
    private CharacterSwapController _characterSwapController = null;

    public Action OnHealthChangedAction;

    #endregion




    #region Health Functions

    /// <summary>
    /// Has the character take damage to their health.
    /// </summary>
    /// <param name="damageArg"></param>
    public void TakeDamage(int damageArg)
    {
        ChangeHealthValue(false, damageArg);
    }

    /// <summary>
    /// Has the character recover health.
    /// </summary>
    /// <param name="healingArg"></param>
    public void HealHealth(int healingArg)
    {
        ChangeHealthValue(true, healingArg);
    }

    /// <summary>
    /// Changes the character's health value based on given properties.
    /// </summary>
    /// <param name="isIncreasingArg"></param>
    /// <param name="changeValueArg"></param>
    private void ChangeHealthValue(bool isIncreasingArg, int changeValueArg)
    {
        // get char data for surface level properties from the char master
        CharacterData charFrontFacingData = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
            _characterMasterController);

        // if adding health
        if (isIncreasingArg)
        {
            // have the surface level char recover health
            charFrontFacingData.characterStats.HealHealth(changeValueArg);
        }
        // else removing health
        else
        {
            // have the surface level char take health damage
            charFrontFacingData.characterStats.TakeDamage(changeValueArg);
        }

        /// set all the swap characters health based on the surface level char's health percentage, 
        /// if appropriate to do so
        MatchHealthPercentageForSwapCharactersIfAppropriate(charFrontFacingData);

        // call health change actions if NOT null
        OnHealthChangedAction?.Invoke();
    }

    /// <summary>
    /// Sets all the swap characters health based on the given char's health percentage, 
    /// if appropriate to do so.
    /// </summary>
    /// <param name="charDataTemplateArg"></param>
    private void MatchHealthPercentageForSwapCharactersIfAppropriate(CharacterData charDataTemplateArg)
    {
        // if NO char swapper reference
        if (_characterSwapController == null)
        {
            // DONT continue code
            return;
        }

        // get health percentage of given char data
        float healthPercentage = charDataTemplateArg.characterStats.GetCurrentHealthPercentage();

        // loop through all swappable characters
        foreach (CharacterData iterCharData in _characterSwapController.SwappableCharacters)
        {
            // set the iterating char's health based on the given char's health percentage
            iterCharData.characterStats.SetHealthToPercentage(healthPercentage);
        }
    }

    /// <summary>
    /// Returns bool that denotes if the given reputation of an attacker allows this char to be 
    /// harmed by attacker.
    /// </summary>
    /// <param name="attackerRepArg"></param>
    /// <returns></returns>
    public bool CanAttackerCauseHarmBasedOnReputation(FactionReputation attackerRepArg)
    {
        // get front facing char data from this character
        CharacterData surfaceLevelCharData = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
            _characterMasterController);
        // if char data retrieved
        if (surfaceLevelCharData != null)
        {
            // returns whether this char's reputation allows the attacker to harm them
            return attackerRepArg.DoesReputationAllowHarm(surfaceLevelCharData.factionReputation);
        }
        // else no char data found
        else
        {
            // return some default bool
            return false;
        }
    }

    #endregion


}
