using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoPouch
{
    #region Class Variables

    [HideInInspector]
    public WeaponTypeSet ammoType;

    public int currentAmmo;
    public int maxAmmo;

    #endregion




    #region Constructors

    public AmmoPouch()
    {
        Setup();
    }

    public AmmoPouch(AmmoPouch templateArg)
    {
        Setup(templateArg);
    }

    public AmmoPouch(AmmoPouchTemplate templateArg)
    {
        Setup(templateArg.template);

        ammoType = new WeaponTypeSet(templateArg.ammoTypeTemplate);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        ammoType = new WeaponTypeSet();

        currentAmmo = 0;
        maxAmmo = 1;
    }

    private void Setup(AmmoPouch templateArg)
    {
        ammoType = new WeaponTypeSet(templateArg.ammoType);

        currentAmmo = templateArg.currentAmmo;
        maxAmmo = templateArg.maxAmmo;
    }

    #endregion




    #region Ammo Functions

    /// <summary>
    /// Returns bool that denotes if have enough ammo.
    /// </summary>
    /// <param name="ammoCostArg"></param>
    /// <returns></returns>
    public bool HaveEnoughAmmo(int ammoCostArg)
    {
        // if ammo IS needed
        if (ammoType.doesNeedAmmo)
        {
            return currentAmmo >= ammoCostArg;
        }
        // else ammo NOT needed
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Reduces the current ammo count.
    /// </summary>
    /// <param name="ammoCostArg"></param>
    public void ReduceCurrentAmmo(int ammoCostArg)
    {
        // if ammo is needed
        if (ammoType.doesNeedAmmo)
        {
            // reduce ammo by given amount while ensureing it doesn't fall below zero
            currentAmmo = Mathf.Max(currentAmmo - ammoCostArg, 0);
        }
    }

    /// <summary>
    /// Increases the current ammo count.
    /// </summary>
    /// <param name="ammoAmountArg"></param>
    public void IncreaseCurrentAmmo(int ammoAmountArg)
    {
        // if ammo is needed
        if (ammoType.doesNeedAmmo)
        {
            // reduce ammo by given amount while ensureing it doesn't go above the max
            currentAmmo = Mathf.Min(currentAmmo + ammoAmountArg, maxAmmo);
        }
    }

    #endregion


}
