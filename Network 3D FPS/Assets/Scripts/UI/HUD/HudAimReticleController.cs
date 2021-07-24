using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudAimReticleController : MonoBehaviour, MMEventListener<UiPlayerCharChangedEvent>
{
    #region Class Variables

    private UIManager _uiManager = null;

    [Header("Ammo Properties")]

    [SerializeField]
    private Image reticle = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all the vars that hold references to singletons
        InitializeSingletonReferences();

        // setup the aiming reticle image
        RefreshAimReticle();
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
    /// Sets up the aiming reticle image.
    /// </summary>
    private void RefreshAimReticle()
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
                // get currently equipped weapon
                WeaponData equippedWep = playerChar.GetEquippedWeapon();

                // if a weapon IS equipped
                if (equippedWep != null)
                {
                    // load the aiming reticle associated with equipped weapon
                    Sprite loadedAimReticle = AssetRefMethods.
                        LoadBundleAssetWeaponTypeReticle(equippedWep.weaponType);

                    // if a reticle was found
                    if (loadedAimReticle != null)
                    {
                        // set reticle image to loaded sprite
                        reticle.sprite = loadedAimReticle;

                        // activate aim reticle
                        reticle.gameObject.SetActive(true);

                        // DONT continue code
                        return;
                    }
                    // else NO reticle was loaded
                    else
                    {
                        // print warning to console
                        Debug.LogWarning("No aiming reticle assoicated with weapon " +
                            $"type: {equippedWep.weaponType.ToString()}");
                    }
                }
            }
        }

        // deactivate aim reticle
        reticle.gameObject.SetActive(false);
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
        playerCharArg.OnCharDataChangedAction += RefreshAimReticle;
        playerCharArg.OnEquippedWeaponSlotNumChangedAction += RefreshAimReticle;

        /*// find the char's action attack component
        CharacterActionAttackController charActionAttack = playerCharArg.
            GetComponent<CharacterActionAttackController>();
        // if comp found
        if (charActionAttack != null)
        {
            charActionAttack.OnCharacterAttackAction += RefreshAmmoBarCount;
        }*/
    }

    public void OnMMEvent(UiPlayerCharChangedEvent eventType)
    {
        // add functions to be called for event player's action events
        AddPlayerEventListening(eventType.playerChar);

        // setup the aiming reticle image
        RefreshAimReticle();
    }

    #endregion


}
