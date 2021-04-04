using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudWeaponModelController : MonoBehaviour, MMEventListener<UiPlayerCharChangedEvent>
{
    #region Class Variables

    private UIManager _uiManager = null;

    [Header("Component References")]

    [SerializeField]
    private Image wepImage = null;
    [SerializeField]
    private Animator wepAnimator = null;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all the vars that hold references to singletons
        InitializeSingletonReferences();

        // setup image comp and sets the image's animator based on the equipped weapon
        RefreshWeaponModel();
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




    #region Weapon Model Functions

    /// <summary>
    /// Setup image comp and sets the image's animator based on the equipped weapon.
    /// </summary>
    private void RefreshWeaponModel()
    {
        // get player char the UI is associated with
        PlayerCharacterMasterController playerChar = _uiManager.UiPlayerCharacter;
        // if player char found
        if (playerChar != null)
        {
            // initialize wep animator as a NULL animator
            AnimatorOverrideController newWepAnim = null;
            // get currently equipped weapon
            WeaponData equippedWep = playerChar.GetEquippedWeapon();

            // if a weapon is equipped
            if (equippedWep != null)
            {
                string wepId = equippedWep.itemInfo.itemId;
                // get equipped weapon's associated HUD model animator
                newWepAnim = AssetRefMethods.LoadBundleAssetHudWeaponModelAnimator(
                    /*equippedWep.itemInfo.itemId*/wepId);
            }

            // turn ON image comp if got valid animator and turn OFF if got null animator
            wepImage.enabled = newWepAnim != null;

            // if got valid aniamtor
            if (newWepAnim != null)
            {
                // set aniamtor's controller to the one retrieved
                wepAnimator.runtimeAnimatorController = newWepAnim;
            }
        }
        // else NO player char retrieved
        else
        {
            // turn OFF weapon image
            wepImage.enabled = false;
        }
    }

    /// <summary>
    /// Plays the attack animation
    /// </summary>
    private void PlayAttackAnimation()
    {
        // trigger animator to play attack animation
        wepAnimator.SetTrigger("Attack");
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
        playerCharArg.OnCharDataChangedAction += RefreshWeaponModel;
        playerCharArg.OnEquippedWeaponSlotNumChangedAction += RefreshWeaponModel;

        // find the char's action attack component
        CharacterActionAttackController charActionAttack = playerCharArg.
            GetComponent<CharacterActionAttackController>();
        // if comp found
        if (charActionAttack != null)
        {
            charActionAttack.OnCharacterAttackAction += PlayAttackAnimation;
        }
    }

    public void OnMMEvent(UiPlayerCharChangedEvent eventType)
    {
        // add functions to be called for event player's action events
        AddPlayerEventListening(eventType.playerChar);

        // setup image comp and sets the image's animator based on the equipped weapon
        RefreshWeaponModel();
    }

    #endregion


}
