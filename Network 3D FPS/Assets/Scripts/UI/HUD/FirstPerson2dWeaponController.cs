using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPerson2dWeaponController : MonoBehaviour, MMEventListener<UiPlayerCharChangedEvent>
{
    #region Class Variables

    private UIManager _uiManager = null;

    [Header("Component References")]

    [SerializeField]
    private Animator wepHeadAnim = null;
    [SerializeField]
    private Animator wepModelAnim = null;

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
    /// Sets up weapon model animator based on the equipped weapon.
    /// </summary>
    private void RefreshWeaponModel()
    {
        // get player char the UI is associated with
        PlayerCharacterMasterController playerChar = _uiManager.UiPlayerCharacter;

        // if player char found
        if (playerChar != null)
        {
            // get currently equipped weapon
            WeaponData equippedWep = playerChar.GetEquippedWeapon();

            // if a weapon is equipped
            if (equippedWep != null)
            {
                // get equipped weapon's ID
                string wepId = equippedWep.itemInfo.itemId;

                // get equipped weapon's associated 1st person model animator
                AnimatorOverrideController newWepAnim = AssetRefMethods.
                    LoadBundleAssetFirstPersonWeaponModelAnimator(equippedWep.weaponType, wepId);

                // if animator found
                if (newWepAnim != null)
                {
                    // turn ON 1st person weapon object
                    wepHeadAnim.gameObject.SetActive(true);

                    // set weapon model animator to retreived weapon animator
                    wepModelAnim.runtimeAnimatorController = newWepAnim;

                    // DONT continue code
                    return;
                }
            }
        }

        // if got here then, turn OFF 1st person weapon object
        wepHeadAnim.gameObject.SetActive(false);
    }

    /// <summary>
    /// Plays the attack animation
    /// </summary>
    private void PlayAttackAnimation()
    {
        // trigger animator to play attack animation
        wepModelAnim.SetTrigger("Attack");
    }

    /// <summary>
    /// Sets the condition for playing weapon moving animation.
    /// </summary>
    private void RefreshWeaponMovingAnimation()
    {
        // get whether char is moving
        bool charMoving = _uiManager.UiPlayerCharacter.CharMovement.IsMoving;

        // set weapon head animation based on char movement
        wepHeadAnim.SetBool("IsMoving", charMoving);
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

        playerCharArg.CharMovement.OnIsMovingChangedAction += RefreshWeaponMovingAnimation;

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

        // setup weapon model animator based on the equipped weapon
        RefreshWeaponModel();
    }

    #endregion


}
