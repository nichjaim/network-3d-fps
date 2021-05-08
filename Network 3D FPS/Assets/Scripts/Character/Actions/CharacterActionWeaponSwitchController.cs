using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterActionWeaponSwitchController : MonoBehaviour
{
    #region Class Variables

    private NetworkManagerCustom _networkManagerCustom = null;

    [Header("Component References")]

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;
    [SerializeField]
    private CharacterMasterController _characterMasterController = null;

    [Header("Weapon Switch Properties")]

    [SerializeField]
    private float weaponScrollCooldownTime = 0.3f;
    private bool onCooldownWeaponScroll = false;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
    }

    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        // switches to next/previous weapon if appropriate player input made
        WeaponSwitchScrollIfPlayerInputted();
        // switches to specific weapon slot if appropriate player input made
        WeaponSwitchSlotIfPlayerInputted();
    }

    private void OnEnable()
    {
        // take weapon scroll OFF cooldown
        onCooldownWeaponScroll = false;
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _networkManagerCustom = (NetworkManagerCustom)NetworkManager.singleton;
    }

    #endregion




    #region Weapon Switch Functions

    /// <summary>
    /// Puts weapon scrolling on cooldown and then take it off after the cooldown time.
    /// </summary>
    private void WeaponScrollCooldown()
    {
        // call internal function as coroutine
        StartCoroutine(WeaponScrollCooldownInternal());
    }

    private IEnumerator WeaponScrollCooldownInternal()
    {
        // put weapon scrolling ON cooldown
        onCooldownWeaponScroll = true;

        // wait cooldown time
        yield return new WaitForSeconds(weaponScrollCooldownTime);

        // take weapon scrolling OFF cooldown
        onCooldownWeaponScroll = false;
    }

    #endregion




    #region Input Functions

    /// <summary>
    /// Switches to the character's next/previous weapon if the appropriate player 
    /// input was made. 
    /// Call in Update().
    /// </summary>
    private void WeaponSwitchScrollIfPlayerInputted()
    {
        // if weapon scrolling is on cooldown
        if (onCooldownWeaponScroll)
        {
            // DONT continue code
            return;
        }

        // initialize minimum input value
        float MINIMUM_INPUT = 0.1f;

        // if next slot input given
        if (Input.mouseScrollDelta.y > MINIMUM_INPUT)
        {
            // switch char's equipped weapon slot to NEXT slot
            _characterMasterController.WeaponSwitchScroll(true);
            // put weapon scrolling on cooldown and then take it off after the cooldown time
            WeaponScrollCooldown();
        }
        // else if previous slot input given
        else if (Input.mouseScrollDelta.y < -MINIMUM_INPUT)
        {
            // switch char's equipped weapon slot to PREVIOUS slot
            _characterMasterController.WeaponSwitchScroll(false);
            // put weapon scrolling on cooldown and then take it off after the cooldown time
            WeaponScrollCooldown();
        }
    }

    /// <summary>
    /// Switches to the character's weapon slot if the appropriate player input was made. 
    /// Call in Update().
    /// </summary>
    private void WeaponSwitchSlotIfPlayerInputted()
    {
        // if slot 1 input given
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // switch char's equipped weapon slot to slot 1
            _characterMasterController.WeaponSwitchSlot(1);
        }
        // else if slot 2 input given
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // switch char's equipped weapon slot to slot 1
            _characterMasterController.WeaponSwitchSlot(2);
        }
        // else if slot 3 input given
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // switch char's equipped weapon slot to slot 1
            _characterMasterController.WeaponSwitchSlot(3);
        }
        // else if slot 4 input given
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // switch char's equipped weapon slot to slot 1
            _characterMasterController.WeaponSwitchSlot(4);
        }
    }

    #endregion


}
