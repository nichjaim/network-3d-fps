using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryAmmo
{
    #region Class Variables

    public List<AmmoPouch> ammoPouches;

    #endregion




    #region Constructors

    public InventoryAmmo()
    {
        Setup();
    }

    public InventoryAmmo(InventoryAmmo templateArg)
    {
        Setup(templateArg);
    }

    public InventoryAmmo(InventoryAmmoTemplate templateArg)
    {
        ammoPouches = new List<AmmoPouch>();
        foreach (AmmoPouchTemplate iterTemp in templateArg.ammoPouchTemplates)
        {
            ammoPouches.Add(new AmmoPouch(iterTemp));
        }
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        ammoPouches = new List<AmmoPouch>();
    }

    private void Setup(InventoryAmmo templateArg)
    {
        ammoPouches = new List<AmmoPouch>();
        foreach (AmmoPouch iterPouch in templateArg.ammoPouches)
        {
            ammoPouches.Add(new AmmoPouch(iterPouch));
        }
    }

    #endregion




    #region Ammo Functions

    public AmmoPouch GetAmmoPouchFromAmmoType(WeaponTypeSet ammoTypeArg)
    {
        return ammoPouches.Find(iterPouch => iterPouch.ammoType.setId == ammoTypeArg.setId);
    }

    #endregion


}
