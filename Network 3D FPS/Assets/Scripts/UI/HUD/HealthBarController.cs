using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : ValueBarController, MMEventListener<UiPlayerCharChangedEvent>
{
    #region Class Variables

    private UIManager _uiManager = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all the vars that hold references to singletons
        InitializeSingletonReferences();

        // fully sets up the health bar properties
        RefreshHealthBarFull();
    }

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up all the vars that hold references to singletons. 
    /// Call in Start(), because need to allow time for singletons to instantiate.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _uiManager = UIManager.Instance;
    }

    #endregion




    #region Ammo Functions

    /// <summary>
    /// Fully sets up the health bar properties.
    /// </summary>
    private void RefreshHealthBarFull()
    {
        // get player char the UI is associated with
        PlayerCharacterMasterController playerChar = _uiManager.UiPlayerCharacter;

        // if player char found
        if (playerChar != null)
        {
            // get front-facing char data
            CharacterData charData = playerChar.CharDataSecondary;

            // if valid char data retrieved
            if (charData != null)
            {
                // get the front-facing char data's stats
                CharacterStats charStats = charData.characterStats;

                // refresh health bar based on char stats
                RefreshBarDimensions(charStats.healthCurrent, charStats.
                    GetTrueMaxHealthWithoutFatigueModifier());

                // turn ON the health bar
                SetBarActivation(true);

                // DONT continue code
                return;
            }
        }

        // getting here means that ammo bar should NOT be used, so turn OFF the health bar
        SetBarActivation(false);
    }

    /// <summary>
    /// Refreshes the current health bar's fill.
    /// </summary>
    private void RefreshHealthBarCount()
    {
        // get the front-facing char data's stats
        CharacterStats charStats = _uiManager.UiPlayerCharacter.CharDataSecondary.
            characterStats;

        // refresh health bar based on char stats
        RefreshBarValue(charStats.healthCurrent);
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        this.MMEventStartListening<UiPlayerCharChangedEvent>();
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        this.MMEventStopListening<UiPlayerCharChangedEvent>();
    }

    /// <summary>
    /// Add functions to be called for given player's action events.
    /// </summary>
    /// <param name="playerCharArg"></param>
    private void AddPlayerEventListening(PlayerCharacterMasterController playerCharArg)
    {
        playerCharArg.OnCharDataSecondaryChangedAction += RefreshHealthBarFull;

        // find the char's health component
        CharacterHealthController charHealth = playerCharArg.
            GetComponentInChildren<CharacterHealthController>(true);
        // if comp found
        if (charHealth != null)
        {
            charHealth.OnHealthChangedAction += RefreshHealthBarCount;
        }
    }

    public void OnMMEvent(UiPlayerCharChangedEvent eventType)
    {
        // add functions to be called for event player's action events
        AddPlayerEventListening(eventType.playerChar);

        // fully sets up the health bar properties
        RefreshHealthBarFull();
    }

    #endregion


}
