using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInventory
{
    #region Class Variables

    public List<WeaponSlotData> weaponSlots;
    public InventoryAmmo inventoryAmmo;

    #endregion




    #region Constructors

    public CharacterInventory()
    {
        Setup();
    }

    public CharacterInventory(CharacterInventory templateArg)
    {
        Setup(templateArg);
    }

    public CharacterInventory(CharacterInventoryTemplate templateArg)
    {
        weaponSlots = new List<WeaponSlotData>();
        foreach (WeaponSlotDataTemplate iterSlotTemp in templateArg.weaponSlotDataTemplates)
        {
            weaponSlots.Add(new WeaponSlotData(iterSlotTemp));
        }

        inventoryAmmo = new InventoryAmmo(templateArg.inventoryAmmoTemplate);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        weaponSlots = new List<WeaponSlotData>();
        inventoryAmmo = new InventoryAmmo();
    }

    private void Setup(CharacterInventory templateArg)
    {
        weaponSlots = new List<WeaponSlotData>();
        foreach (WeaponSlotData iterSlot in templateArg.weaponSlots)
        {
            weaponSlots.Add(new WeaponSlotData(iterSlot));
        }

        //inventoryAmmo = new InventoryAmmo(templateArg.inventoryAmmo);
    }

    #endregion




    #region Inventory Functions

    /// <summary>
    /// Returns the weapon slot associated wtih the given weapon slot number.
    /// </summary>
    /// <param name="slotNumberArg"></param>
    /// <returns></returns>
    public WeaponSlotData GetWeaponSlot(int slotNumberArg)
    {
        // if given invalid weapon slot number OR weapon slot that does not currently have an entry
        if (slotNumberArg <= 0 || slotNumberArg > weaponSlots.Count)
        {
            // return null weapon
            return null;
        }
        else
        {
            // return associated weapon slot
            return weaponSlots[slotNumberArg - 1];
        }
    }

    /// <summary>
    /// Adds the given weapon slot to the list of available weapon slots.
    /// </summary>
    /// <param name="templateArg"></param>
    public void AddWeaponSlot(WeaponSlotDataTemplate templateArg)
    {
        weaponSlots.Add(new WeaponSlotData(templateArg));
    }

    /// <summary>
    /// Adds given weapon to an available appropriate weapon slot, if possible.
    /// </summary>
    /// <param name="wepArg"></param>
    public void AddWeapon(WeaponData wepArg)
    {
        // loop through all weapon slots
        foreach (WeaponSlotData iterWepSlot in weaponSlots)
        {
            // if iterating weapon slot is empty AND is allows the given weapon's type
            if (iterWepSlot.slotWeapon == null && 
                iterWepSlot.requiredWeaponTypeSet.IsWeaponTypeInSet(wepArg.weaponType))
            {
                // place given weapon in iterating weapon slot
                iterWepSlot.slotWeapon = new WeaponData(wepArg);

                // DONT continue code
                return;
            }
        }

        // if got here then no room for weapon. print warning to console.
        Debug.LogWarning("No room for weapon being added. " +
            $"ID: {wepArg.itemInfo.itemId}");
    }

    #endregion


}
