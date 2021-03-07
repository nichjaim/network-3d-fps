using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    #region Class Variables

    public string weaponId;
    public string weaponName;

    public WeaponType weaponType;

    #endregion




    #region Constructors

    public WeaponData()
    {
        Setup();
    }

    public WeaponData(WeaponData templateArg)
    {
        Setup(templateArg);
    }

    public WeaponData(WeaponDataTemplate templateArg)
    {
        if (templateArg == null)
        {
            // DONT continue code
            return;
        }

        Setup(templateArg.template);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        weaponId = "0";
        weaponName = "NAME";

        weaponType = WeaponType.None;
    }

    private void Setup(WeaponData templateArg)
    {
        weaponId = templateArg.weaponId;
        weaponName = templateArg.weaponName;

        weaponType = templateArg.weaponType;
    }

    #endregion


}
