using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    #region Class Variables

    public InventoryItemInfo itemInfo;

    public WeaponType weaponType;

    public ProjectileFireMethodType fireMethod;

    public WeaponStats weaponStats;

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
        Setup(templateArg.template);

        itemInfo.SetupPrefixName(templateArg.potentialPrefixNames);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        itemInfo = new InventoryItemInfo();

        weaponType = WeaponType.None;

        fireMethod = ProjectileFireMethodType.None;

        weaponStats = new WeaponStats();
    }

    private void Setup(WeaponData templateArg)
    {
        itemInfo = new InventoryItemInfo(templateArg.itemInfo);

        weaponType = templateArg.weaponType;

        fireMethod = templateArg.fireMethod;

        weaponStats = new WeaponStats(templateArg.weaponStats);
    }

    #endregion


}
