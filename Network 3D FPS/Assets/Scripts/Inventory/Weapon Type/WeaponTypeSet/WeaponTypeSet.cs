using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponTypeSet
{
    #region Class Variables

    public string setId;
    public string setName;

    public List<WeaponType> weaponTypes;

    #endregion




    #region Constructors

    public WeaponTypeSet()
    {
        Setup();
    }

    public WeaponTypeSet(WeaponTypeSet templateArg)
    {
        Setup(templateArg);
    }

    public WeaponTypeSet(WeaponTypeSetTemplate templateArg)
    {
        Setup(templateArg.template);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        setId = "0";
        setName = "NAME";

        weaponTypes = new List<WeaponType>();
    }

    private void Setup(WeaponTypeSet templateArg)
    {
        setId = templateArg.setId;
        setName = templateArg.setName;

        weaponTypes = new List<WeaponType>();
        foreach (WeaponType iterType in templateArg.weaponTypes)
        {
            weaponTypes.Add(iterType);
        }
    }

    #endregion




    #region Weapon Type Functions

    /// <summary>
    /// Returns bool that denotes if this set contaisn the given type.
    /// </summary>
    /// <param name="weaponTypeArg"></param>
    /// <returns></returns>
    public bool IsWeaponTypeInSet(WeaponType weaponTypeArg)
    {
        return weaponTypes.Contains(weaponTypeArg);
    }

    #endregion


}
