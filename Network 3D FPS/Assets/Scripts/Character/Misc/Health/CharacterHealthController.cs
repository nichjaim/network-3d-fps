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
    protected CharacterMasterController _characterMasterController = null;

    [SerializeField]
    private CharacterSwapController _characterSwapController = null;

    [Tooltip("The point from which an action text popup will spawn when a position is not specified.")]
    [SerializeField]
    private Transform defaultActionTextPopupPointArg = null;

    public Action OnHealthChangedAction;
    public Action OnOutOfHealthAction;

    public Action OnDamageTaken;
    public Action OnHealthHealed;

    /// positive for increased dmg taken, negative for decreased damage taken. 
    /// Used for status effects.
    private float damageTakenChangePercentage = 0f;
    /// list of colors that the damage popup will temporarily be changed to. 
    /// Used for status effects.
    private List<Color> temporaryDamagePopupColors = new List<Color>();

    //private StopTimer postDamageInvinciblilityTimer = null;
    private bool inPostDamageInvincibility = false;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
    }

    private void OnEnable()
    {
        // ensure char is out of post-damage invincibility
        inPostDamageInvincibility = false;
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    protected virtual void InitializeSingletonReferences()
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
    public void TakeDamage(CharacterMasterController damageInflicterArg, int damageArg, 
        Vector3 impactPointArg)
    {
        ChangeHealthValue(damageInflicterArg, false, damageArg, impactPointArg);
    }

    public void TakeDamage(CharacterMasterController damageInflicterArg, int damageArg)
    {
        TakeDamage(damageInflicterArg, damageArg, defaultActionTextPopupPointArg.position);
    }

    /// <summary>
    /// Have the character recover health.
    /// </summary>
    /// <param name="healingInflicterArg"></param>
    /// <param name="healingArg"></param>
    public void HealHealth(CharacterMasterController healingInflicterArg, int healingArg, 
        Vector3 impactPointArg)
    {
        ChangeHealthValue(healingInflicterArg, true, healingArg, impactPointArg);
    }

    public void HealHealth(CharacterMasterController healingInflicterArg, int healingArg)
    {
        HealHealth(healingInflicterArg, healingArg, defaultActionTextPopupPointArg.position);
    }

    /// <summary>
    /// Changes the character's health value based on given properties.
    /// </summary>
    /// <param name="changeInflicterArg"></param>
    /// <param name="isIncreasingArg"></param>
    /// <param name="changeValueArg"></param>
    /// <param name="impactPointArg"></param>
    private void ChangeHealthValue(CharacterMasterController changeInflicterArg, 
        bool isIncreasingArg, int changeValueArg, Vector3 impactPointArg)
    {
        // if taking damage AND in post-damage invincibility
        if (!isIncreasingArg && inPostDamageInvincibility)
        {
            // DONT continue code
            return;
        }

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

            // call health healed actions if NOT null
            OnHealthHealed?.Invoke();
        }
        // else removing health
        else
        {
            // have the surface level char take health damage
            actualChangeValue = charFrontFacingData.characterStats.TakeDamage(changeValueArg);

            // call damage taken actions if NOT null
            OnDamageTaken?.Invoke();
        }

        /// get the char's post-damage invincibility time. 
        /// NOTE: this code stuff done here because healing should also give char a short invincibility
        float dmgInvincTime = charFrontFacingData.characterStats.postDamageInvincibilityTime;
        // if char has post-damage invincibility
        if (dmgInvincTime > 0f)
        {
            // make char unable to take damage for given time
            ApplyPostDamageInvincibility(dmgInvincTime);
        }

        // spawns and sets up the action text popup, if appropriate to do so
        SetupActionTextPopupIfAppropriate(changeInflicterArg, isIncreasingArg, actualChangeValue, impactPointArg);

        /// set all the swap characters health based on the surface level char's health percentage, 
        /// if appropriate to do so
        MatchHealthPercentageForSwapCharactersIfAppropriate(charFrontFacingData);

        // call health change actions if NOT null
        OnHealthChangedAction?.Invoke();

        // if character has run out of health
        if (charFrontFacingData.characterStats.IsOutOfHealth())
        {
            // performs the necessary operations when the character loses all their health
            ProcessDeath();
        }
    }

    /// <summary>
    /// Performs the necessary operations when the character loses all their health.
    /// </summary>
    protected virtual void ProcessDeath()
    {
        // call out-of-health change actions if NOT null
        OnOutOfHealthAction?.Invoke();
    }

    /// <summary>
    /// Resets the character's health back to max.
    /// </summary>
    public void ResetHealth()
    {
        // get char data for surface level properties from the char master
        CharacterData charFrontFacingData = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
            _characterMasterController);

        // if no char data set yet
        if (charFrontFacingData == null)
        {
            // DONT continue code
            return;
        }

        // remove all char's health fatigue
        charFrontFacingData.characterStats.RemoveAllHealthFatigue();
        // have the surface level char recover all health
        charFrontFacingData.characterStats.FillHealthToMax();

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

    /*private void IncreasePostDamageInvincibilityCooldownTimersByTimePassed()
    {

    }*/

    /// <summary>
    /// Puts character in a post-damage invincibility and then removing them from it after 
    /// the given time.
    /// </summary>
    /// <param name="timeArg"></param>
    private void ApplyPostDamageInvincibility(float timeArg)
    {
        StartCoroutine(ApplyPostDamageInvincibilityInternal(timeArg));
    }

    private IEnumerator ApplyPostDamageInvincibilityInternal(float timeArg)
    {
        // put char IN post-damage invincibility
        inPostDamageInvincibility = true;

        // wait given time
        yield return new WaitForSeconds(timeArg);

        // take char OUT of post-damage invincibility
        inPostDamageInvincibility = false;
    }

    #endregion




    #region Popup Functions

    /// <summary>
    /// Spawns a pooled action text popup.
    /// </summary>
    /// <returns></returns>
    private ActionTextPopupController SpawnActionTextPopup(Vector3 spawnPosArg)
    {
        // get pooled object
        GameObject spawnedPopupObj = _spawnManager.GetPooledActionTextPopup(
            spawnPosArg,
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
        bool isIncreasingArg, int changeValueArg, Vector3 spawnPointArg)
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
        ActionTextPopupController spawnedPopup = SpawnActionTextPopup(spawnPointArg);

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
