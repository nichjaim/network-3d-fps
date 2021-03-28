﻿using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBarController : ValueBarController, MMEventListener<UiPlayerCharChangedEvent>
{
    #region Class Variables

    private UIManager _uiManager = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all the vars that hold references to singletons
        InitializeSingletonReferences();

        // fully sets up the ammo bar properties
        RefreshAmmoBarFull();
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
    /// Fully sets up the ammo bar properties.
    /// </summary>
    private void RefreshAmmoBarFull()
    {
        // get player char the UI is associated with
        PlayerCharacterMasterController playerChar = _uiManager.UiPlayerCharacter;

        // if player char found
        if (playerChar != null)
        {
            // get weapon slot currently being used
            WeaponSlotData equippedWepSlot = playerChar.GetEquippedWeaponSlot();

            // if weapon slot found
            if (equippedWepSlot != null)
            {
                // if wep slot's wep type set DOES need ammo to be used (i.e. NOT infinite ammo)
                if (equippedWepSlot.requiredWeaponTypeSet.doesNeedAmmo)
                {
                    // get currently equipped ammo pouch
                    AmmoPouch equippedAmmoPouch = playerChar.GetEquippedAmmoPouch();

                    // if pouch found
                    if (equippedAmmoPouch != null)
                    {
                        // turn ON the ammo bar
                        SetBarActivation(true);

                        // refresh ammo bar based on ammo pouch
                        RefreshBarDimensions(equippedAmmoPouch.currentAmmo, equippedAmmoPouch.maxAmmo);

                        // DONT continue code
                        return;
                    }
                }
            }
        }

        // getting here means that ammo bar should NOT be used, so turn OFF the ammo bar
        SetBarActivation(false);
    }

    /// <summary>
    /// Refreshes the current ammo bar's fill.
    /// </summary>
    private void RefreshAmmoBarCount()
    {
        // get currently equipped ammo pouch
        AmmoPouch equippedAmmoPouch = _uiManager.UiPlayerCharacter.GetEquippedAmmoPouch();

        // if pouch found
        if (equippedAmmoPouch != null)
        {
            // refresh ammo bar based on ammo pouch
            RefreshBarValue(equippedAmmoPouch.currentAmmo);
        }
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
        playerCharArg.OnCharDataChangedAction += RefreshAmmoBarFull;
        playerCharArg.OnEquippedWeaponSlotNumChangedAction += RefreshAmmoBarFull;

        // find the char's action attack component
        CharacterActionAttackController charActionAttack = playerCharArg.
            GetComponent<CharacterActionAttackController>();
        // if comp found
        if (charActionAttack != null)
        {
            charActionAttack.OnCharacterAttackAction += RefreshAmmoBarCount;
        }
    }

    public void OnMMEvent(UiPlayerCharChangedEvent eventType)
    {
        // add functions to be called for event player's action events
        AddPlayerEventListening(eventType.playerChar);

        // fully sets up the ammo bar properties
        RefreshAmmoBarFull();
    }

    #endregion


}