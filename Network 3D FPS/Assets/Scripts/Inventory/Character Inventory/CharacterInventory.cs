using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInventory
{
    #region Class Variables

    public List<WeaponSlotData> weaponSlots;

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
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        weaponSlots = new List<WeaponSlotData>();
    }

    private void Setup(CharacterInventory templateArg)
    {
        weaponSlots = new List<WeaponSlotData>();
        foreach (WeaponSlotData iterSlot in templateArg.weaponSlots)
        {
            weaponSlots.Add(new WeaponSlotData(iterSlot));
        }
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

    #endregion


}
