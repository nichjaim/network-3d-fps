using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartyInventory
{
    #region Class Variables

    public int inventorySize = 1;
    public List<WeaponData> weaponInventory = new List<WeaponData>();

    #endregion




    #region Constructors

    public PartyInventory()
    {
        Setup();
    }

    public PartyInventory(PartyInventory templateArg)
    {
        Setup(templateArg);
    }

    public PartyInventory(PartyInventoryTemplate templateArg)
    {
        Setup(templateArg.template);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        inventorySize = 1;
        weaponInventory = new List<WeaponData>();
    }

    private void Setup(PartyInventory templateArg)
    {
        inventorySize = templateArg.inventorySize;

        weaponInventory = new List<WeaponData>();
        foreach (WeaponData iterWep in templateArg.weaponInventory)
        {
            weaponInventory.Add(new WeaponData(iterWep));
        }
    }

    #endregion


}
