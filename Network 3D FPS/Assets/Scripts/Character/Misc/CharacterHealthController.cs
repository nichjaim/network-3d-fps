using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterHealthController : MonoBehaviour
{
    #region Class Variables

    private SpawnManager _spawnManager = null;

    [Header("Component References")]

    [SerializeField]
    private CharacterMasterController _characterMasterController = null;

    [SerializeField]
    private CharacterSwapController _characterSwapController = null;

    public Action OnHealthChangedAction;
    public Action OnOutOfHealthAction;

    /// positive for increased dmg taken, negative for decreased damage taken. 
    /// Used for status effects.
    private float damageTakenChangePercentage = 0f;
    /// list of colors that the damage popup will temporarily be changed to. 
    /// Used for status effects.
    private List<Color> temporaryDamagePopupColors = new List<Color>();

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _spawnManager = GameManager.Instance.SpawnManager;
    }

    #endregion




    #region Health Functions

    /// <summary>
    /// Have the character take damage to their health.
    /// </summary>
    /// <param name="damageInflicterArg"></param>
    /// <param name="damageArg"></param>
    public void TakeDamage(CharacterMasterController damageInflicterArg, int damageArg)
    {
        ChangeHealthValue(damageInflicterArg, false, damageArg);
    }

    /// <summary>
    /// Have the character recover health.
    /// </summary>
    /// <param name="healingInflicterArg"></param>
    /// <param name="healingArg"></param>
    public void HealHealth(CharacterMasterController healingInflicterArg, int healingArg)
    {
        ChangeHealthValue(healingInflicterArg, true, healingArg);
    }

    /// <summary>
    /// Changes the character's health value based on given properties.
    /// </summary>
    /// <param name="changeInflicterArg"></param>
    /// <param name="isIncreasingArg"></param>
    /// <param name="changeValueArg"></param>
    private void ChangeHealthValue(CharacterMasterController changeInflicterArg, 
        bool isIncreasingArg, int changeValueArg)
    {
        // get char data for surface level properties from the char master
        CharacterData charFrontFacingData = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
            _characterMasterController);

        // initialize var to hold the actual change value, true value based on char stats
        int actualChangeValue = changeValueArg;

        // if taking damage
        if (!isIncreasingArg)
        {
            // get the damage change when damage taken change is taken into account
            float dmgChangeAmountUnrounded = changeValueArg * damageTakenChangePercentage;
            // round the damage change value
            int dmgChangeAmount = Mathf.RoundToInt(dmgChangeAmountUnrounded);
            /// add the damage change to the current actual change value, while ensuring the damage 
            /// does not fall below zero. 
            /// NOTE: a negative change should decrease damage taken and positive will increase
            actualChangeValue = Mathf.Max(actualChangeValue + dmgChangeAmount, 0);
        }

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
            actualChangeValue = charFrontFacingData.characterStats.TakeDamage(changeValueArg);
        }

        // spawns and sets up the action text popup, if appropriate to do so
        SetupActionTextPopupIfAppropriate(changeInflicterArg, isIncreasingArg, actualChangeValue);

        /// set all the swap characters health based on the surface level char's health percentage, 
        /// if appropriate to do so
        MatchHealthPercentageForSwapCharactersIfAppropriate(charFrontFacingData);

        // call health change actions if NOT null
        OnHealthChangedAction?.Invoke();

        if (charFrontFacingData.characterStats.IsOutOfHealth())
        {
            // call out-of-health change actions if NOT null
            OnOutOfHealthAction?.Invoke();
        }
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

    public bool AreFactionAllies(FactionReputation attackerRepArg)
    {
        // get front facing char data from this character
        CharacterData surfaceLevelCharData = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
            _characterMasterController);
        // if char data retrieved
        if (surfaceLevelCharData != null)
        {
            return surfaceLevelCharData.factionReputation.AreFactionAllies(attackerRepArg);
        }
        // else no char data found
        else
        {
            // return some default bool
            return false;
        }
    }

    /*private void ProcessCharacterOutOfHealth()
    {

    }*/

    #endregion




    #region Popup Functions

    /// <summary>
    /// Spawns a pooled action text popup.
    /// </summary>
    /// <returns></returns>
    private ActionTextPopupController SpawnActionTextPopup()
    {
        // get pooled object
        GameObject spawnedPopupObj = _spawnManager.GetPooledActionTextPopup(
            _characterMasterController.transform.position,
            _characterMasterController.transform.rotation);

        // turn ON pooled object
        spawnedPopupObj.SetActive(true);

        // get popup component from spawned object
        ActionTextPopupController spawnedPopup = spawnedPopupObj.
            GetComponent<ActionTextPopupController>();

        // return pooled popup
        return spawnedPopup;
    }

    /// <summary>
    /// Spawns and sets up the action text popup, if appropriate to do so.
    /// </summary>
    /// <param name="changeInflicterArg"></param>
    /// <param name="isIncreasingArg"></param>
    /// <param name="changeValueArg"></param>
    private void SetupActionTextPopupIfAppropriate(CharacterMasterController changeInflicterArg,
        bool isIncreasingArg, int changeValueArg)
    {
        // if NO inflicter
        if (changeInflicterArg == null)
        {
            // DONT continue code
            return;
        }

        // if inflicter is NOT a player
        if ((changeInflicterArg as PlayerCharacterMasterController) == null)
        {
            // DONT continue code
            return;
        }

        // if inflicter is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(changeInflicterArg.netIdentity))
        {
            // DONT continue code
            return;
        }

        // if self-infliction
        if (_characterMasterController == changeInflicterArg)
        {
            // DONT continue
            return;
        }

        // spawn an action text popup
        ActionTextPopupController spawnedPopup = SpawnActionTextPopup();

        // if adding health
        if (isIncreasingArg)
        {
            // setup action text popup to denote healing
            spawnedPopup.Setup("+", Color.green);
        }
        // else removing health
        else
        {
            // initialize damage popup color
            Color damagePopupColor = GetTemporaryDamagePopupColorsMix();
            // if no actual damage was taken
            if (changeValueArg <= 0)
            {
                damagePopupColor = Color.grey;
            }

            // setup action text popup to denote healing
            spawnedPopup.Setup(changeValueArg.ToString(), damagePopupColor);
        }
    }

    #endregion




    #region Status Effect Functions

    /// <summary>
    /// Returns a mix of all the temporary damage popup colors. 
    /// Returns the default damage popup color if there are NO temporary colors.
    /// </summary>
    /// <returns></returns>
    private Color GetTemporaryDamagePopupColorsMix()
    {
        return GeneralMethods.GetColorsMix(temporaryDamagePopupColors, Color.white);
    }

    /// <summary>
    /// Adds color to temporary damage popup colors.
    /// </summary>
    /// <param name="colorArg"></param>
    public void AddTemporaryDamagePopupColor(Color colorArg)
    {
        // add given color to temporary damage popup colors
        temporaryDamagePopupColors.Add(colorArg);
    }

    /// <summary>
    /// Removes color to temporary temporary damage popup colors.
    /// </summary>
    /// <param name="colorArg"></param>
    public void RemoveTemporaryDamagePopupColor(Color colorArg)
    {
        // remove given color to temporary damage popup colors
        temporaryDamagePopupColors.Remove(colorArg);
    }

    /// <summary>
    /// Adds given percent value to the damage taken chance percentage. 
    /// Positive value means more damage taken, negative means less damage.
    /// </summary>
    /// <param name="percentValueArg"></param>
    public void AddDamageTakenChangePercentage(float percentValueArg)
    {
        damageTakenChangePercentage += percentValueArg;
    }

    #endregion




    /*#region Process Out Of Health Functions

    /// <summary>
    /// Performs necessary processes when a character runs out of health.
    /// </summary>
    private void ProcessOutOfHealth()
    {
        if (NetworkClient.isConnected)
        {
            CmdProcessOutOfHealth();
        }
        else
        {
            ProcessOutOfHealthInternal();
        }
    }

    [Command]
    private void CmdProcessOutOfHealth()
    {
        SpawnProjectileInternal();
    }

    private void ProcessOutOfHealthInternal()
    {
        // get object from given pooler
        GameObject projectileObj = _weakenProjectilePooler.GetFromPool(_castPoint.position,
            GetRandomProjectileRotation());

        // spawn pooled object on network
        NetworkServer.Spawn(projectileObj);

        // get projectile from pooled object
        WeakeningProjectileController projectile = projectileObj.GetComponent<WeakeningProjectileController>();
        // if no such component found
        if (projectile == null)
        {
            // print warning to console
            Debug.LogWarning("Projectile object does NOT have ProjectileController component! " +
                $"Object name: {projectileObj.name}");

            // DONT continue code
            return;
        }

        // setup projectile data
        projectile.SetupProjectile(_casterCharMaster);
        projectile.TargetPenetrationMax = targetPenetration;
        projectile.SetupWeakeningProjectile(damageTakenIncreasePercentage, effectDuration);
    }

    #endregion*/


}
