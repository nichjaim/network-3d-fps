using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInventory : MonoBehaviour
{
    #region Class Variables

    [HideInInspector]
    public List<string> weaponSlots; //TEMP VAR TYPE

    #endregion




    #region Inventory Functions

    /// <summary>
    /// Returns the weapon associated wtih the given weapon slot number.
    /// </summary>
    /// <param name="weaponSlotNumArg"></param>
    /// <returns></returns>
    public string GetWeaponFromWeaponSlot(int weaponSlotNumArg)
    {
        // if given invalid weapon slot number
        if (weaponSlotNumArg <= 0)
        {
            // print warning to console
            Debug.LogWarning($"Given invalid weaponSlotNumArg of {weaponSlotNumArg}");
            // return null weapon
            return string.Empty;
        }
        // else if given weapon slot number for slot that does not currently have an entry
        else if (weaponSlots.Count > weaponSlotNumArg)
        {
            // return null weapon
            return string.Empty;
        }
        // else given weapon slot number for slot with existing entry
        else
        {
            // return associated weapon
            return weaponSlots[weaponSlotNumArg - 1];
        }
    }

    #endregion


}
