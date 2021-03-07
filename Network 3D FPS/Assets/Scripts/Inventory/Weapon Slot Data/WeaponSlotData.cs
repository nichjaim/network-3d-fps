using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponSlotData
{
    #region Class Variables

    public WeaponData slotWeapon;
    public WeaponTypeSet requiredWeaponTypeSet;

    #endregion




    #region Constructors

    public WeaponSlotData()
    {
        Setup();
    }

    public WeaponSlotData(WeaponSlotData templateArg)
    {
        Setup(templateArg);
    }

    public WeaponSlotData(WeaponSlotDataTemplate templateArg)
    {
        slotWeapon = new WeaponData(templateArg.weaponDataTemplate);
        requiredWeaponTypeSet = new WeaponTypeSet(templateArg.weaponTypeSetTemplate);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        slotWeapon = null;
        requiredWeaponTypeSet = null;
    }

    private void Setup(WeaponSlotData templateArg)
    {
        slotWeapon = new WeaponData(templateArg.slotWeapon);
        requiredWeaponTypeSet = new WeaponTypeSet(templateArg.requiredWeaponTypeSet);
    }

    #endregion


}
