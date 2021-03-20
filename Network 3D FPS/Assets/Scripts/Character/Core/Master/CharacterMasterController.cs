using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class CharacterMasterController : NetworkBehaviour
{
    #region Class Variables

    [SyncVar(hook = nameof(OnCharDataChanged))]
    private CharacterData charData = null;
    public Action OnCharDataChangedAction;

    [SyncVar(hook = nameof(OnEquippedWeaponSlotNumChanged))]
    private int equippedWeaponSlotNum = 1;
    public Action OnEquippedWeaponSlotNumChangedAction;

    #endregion




    #region Weapon Functions

    /// <summary>
    /// Switches the equipped weapon slot to the next/previous slot based on given bool.
    /// </summary>
    /// <param name="switchToNextArg"></param>
    public void WeaponSwitchScroll(bool switchToNextArg)
    {
        // if no valid char data to get inventory from
        if (charData == null)
        {
            // DONT continue code
            return;
        }

        // get equipped weapon slot
        int newEquippedWepSlot = GetEquippedWeaponSlotNum();
        // get total number of weapon slots
        int numWeaponSlots = charData.characterInventory.weaponSlots.Count;

        // if switching to NEXT weapon slot
        if (switchToNextArg)
        {
            // increment the new equip slot number
            newEquippedWepSlot++;
            // if wep slot rises above the valid slots
            if (newEquippedWepSlot > numWeaponSlots)
            {
                // wrap wep slot back down to lowest slot
                newEquippedWepSlot = 1;
            }
        }
        // else switching to PREVIOUS weapon slot
        else
        {
            // decrement the new equip slot number
            newEquippedWepSlot--;
            // if wep slot falls belows the valid slots
            if (newEquippedWepSlot < 1)
            {
                // wrap wep slot back up to highest slot
                newEquippedWepSlot = numWeaponSlots;
            }
        }

        // set the equipped weapon slot num to the new setup num
        SetEquippedWeaponSlotNum(newEquippedWepSlot);
    }

    /// <summary>
    /// Switches the equipped weapon slot based on given slot number.
    /// </summary>
    /// <param name="weaponSlotArg"></param>
    public void WeaponSwitchSlot(int weaponSlotArg)
    {
        // if no valid char data to get inventory from
        if (charData == null)
        {
            // DONT continue code
            return;
        }

        // get new slot num from given slot while ensuring it is a valid slot number
        int newEquippedWepSlot = Mathf.Clamp(weaponSlotArg, 1, 
            charData.characterInventory.weaponSlots.Count);
        // set the equipped weapon slot num to the new setup num
        SetEquippedWeaponSlotNum(newEquippedWepSlot);
    }

    /// <summary>
    /// Returns the currently equipped weapon.
    /// </summary>
    /// <returns></returns>
    public WeaponData GetEquippedWeapon()
    {
        // get currently equipped weapon slot
        WeaponSlotData equippedWepSlot = GetEquippedWeaponSlot();
        // if no weapon slot found
        if (equippedWepSlot == null)
        {
            // return a null weapon
            return null;
        }

        // return found weapon slot's weapon
        return equippedWepSlot.slotWeapon;
    }

    /// <summary>
    /// Returns the weapon slot that is currently being used.
    /// </summary>
    /// <returns></returns>
    private WeaponSlotData GetEquippedWeaponSlot()
    {
        // if no char data is set
        if (charData == null)
        {
            // return a null weapon slot
            return null;
        }

        // return currently equipped weapon slot
        return charData.characterInventory.GetWeaponSlot(
            equippedWeaponSlotNum);
    }

    private AmmoPouch GetEquippedAmmoPouch()
    {
        // get currently equipped weapon slot
        WeaponSlotData equippedWepSlot = GetEquippedWeaponSlot();
        // if no weapon slot found
        if (equippedWepSlot == null)
        {
            // return a null weapon pouch
            return null;
        }

        InventoryAmmo invAmmo = charData.characterInventory.inventoryAmmo;
        return invAmmo.GetAmmoPouchFromAmmoType(equippedWepSlot.requiredWeaponTypeSet);
    }

    /// <summary>
    /// Returns bool that denotes if char has a weapon equipped.
    /// </summary>
    /// <returns></returns>
    public bool HaveWeaponEquipped()
    {
        return GetEquippedWeapon() != null;
    }

    /// <summary>
    /// Returns bool that denotes if char has enough ammo to use equipped weapon.
    /// </summary>
    /// <returns></returns>
    public bool HaveEnoughAmmo()
    {
        // if no weapon equipped
        if (!HaveWeaponEquipped())
        {
            // return that do NOT have enough ammo
            return false;
        }

        return GetEquippedAmmoPouch().HaveEnoughAmmo(GetEquippedWeapon().weaponStats.ammoPerUse);
    }

    /// <summary>
    /// Reduces amount of ammo currently held based on equipped weapon. 
    /// Reutrns bool that denotes if action successful.
    /// </summary>
    /// <returns></returns>
    public bool ReduceEquippedAmmoByEquippedWeapon()
    {
        // get currently equipped ammo pouch
        AmmoPouch ammoPouch = GetEquippedAmmoPouch();
        // if no ammo pouch found
        if (ammoPouch == null)
        {
            // print warning to console
            Debug.LogWarning("No ammo pouch found to reduce ammo from!");
            // return that action failed
            return false;
        }

        // reduce ammo based on equipped weapon
        ammoPouch.ReduceCurrentAmmo(GetEquippedWeapon().weaponStats.ammoPerUse);
        // return that action succeeded
        return true;
    }

    #endregion




    #region Sync Functions

    public void OnCharDataChanged(CharacterData oldCharArg,
        CharacterData newCharArg)
    {
        // call char data change actions if NOT null
        OnCharDataChangedAction?.Invoke();
    }

    public void OnEquippedWeaponSlotNumChanged(int oldNumArg,
        int newNumArg)
    {
        // call equipped weapon slot num change actions if NOT null
        OnEquippedWeaponSlotNumChangedAction?.Invoke();
    }

    #endregion




    #region Getter Functions

    public CharacterData GetCharData()
    {
        return charData;
    }

    public int GetEquippedWeaponSlotNum()
    {
        return equippedWeaponSlotNum;
    }

    #endregion




    #region Set CharData Functions

    public void SetCharData(CharacterData charDataArg)
    {
        // call internal function as coroutine
        StartCoroutine(SetCharDataInternal(charDataArg));
    }

    private IEnumerator SetCharDataInternal(CharacterData charDataArg)
    {
        /// wait till network properties setup (NOTE: not sure why or if should 
        /// do this, but seems to fix some things, idk)
        yield return new WaitForEndOfFrame();

        if (NetworkClient.isConnected)
        {
            CmdSetCharData(charDataArg);
        }
        else
        {
            SetCharDataMain(charDataArg);
            // call sync hook method, as won't be called in offline mode
            OnCharDataChanged(charDataArg, charDataArg);
        }
    }

    [Command(ignoreAuthority = true)]
    private void CmdSetCharData(CharacterData charDataArg)
    {
        SetCharDataMain(charDataArg);
    }

    private void SetCharDataMain(CharacterData charDataArg)
    {
        charData = charDataArg;
    }

    #endregion




    #region Set EquippedWeaponSlotNum Functions

    public void SetEquippedWeaponSlotNum(int slotNumArg)
    {
        // call internal function as coroutine
        StartCoroutine(SetEquippedWeaponSlotNumInternal(slotNumArg));
    }

    private IEnumerator SetEquippedWeaponSlotNumInternal(int slotNumArg)
    {
        /// wait till network properties setup (NOTE: not sure why or if should 
        /// do this, but seems to fix some things, idk)
        yield return new WaitForEndOfFrame();

        if (NetworkClient.isConnected)
        {
            CmdSetEquippedWeaponSlotNum(slotNumArg);
        }
        else
        {
            SetEquippedWeaponSlotNumMain(slotNumArg);
            // call sync hook method, as won't be called in offline mode
            OnEquippedWeaponSlotNumChanged(slotNumArg, slotNumArg);
        }
    }

    [Command(ignoreAuthority = true)]
    private void CmdSetEquippedWeaponSlotNum(int slotNumArg)
    {
        SetEquippedWeaponSlotNumMain(slotNumArg);
    }

    private void SetEquippedWeaponSlotNumMain(int slotNumArg)
    {
        equippedWeaponSlotNum = slotNumArg;
    }

    #endregion


}
