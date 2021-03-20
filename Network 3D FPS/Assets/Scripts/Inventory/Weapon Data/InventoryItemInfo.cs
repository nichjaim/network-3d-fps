using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemInfo
{
    #region Class Variables

    public string itemId;
    public string itemName;
    [TextArea(3, 15)]
    public string itemDescription;

    // the name that goes before the base name
    public string itemPrefixName;

    public RarityTier itemRarity;

    #endregion




    #region Constructors

    public InventoryItemInfo()
    {
        Setup();
    }

    public InventoryItemInfo(InventoryItemInfo templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        itemId = "0";
        itemName = "NAME";
        itemDescription = "DESCRIPTION";

        itemPrefixName = "PREFIX-NAME";

        itemRarity = new RarityTier();
    }

    private void Setup(InventoryItemInfo templateArg)
    {
        itemId = templateArg.itemId;
        itemName = templateArg.itemName;
        itemDescription = templateArg.itemDescription;

        itemPrefixName = templateArg.itemPrefixName;

        itemRarity = new RarityTier(templateArg.itemRarity);
    }

    public void SetupPrefixName(List<string> potentialNamesArg)
    {
        itemPrefixName = GeneralMethods.GetRandomStringListEntry(potentialNamesArg);
    }

    #endregion




    #region Info Functions

    /// <summary>
    /// Returns the item's full name.
    /// </summary>
    /// <returns></returns>
    public string GetFullItemName()
    {
        // initialize return name as empty string
        string fullItemName = string.Empty;

        // if have a rarity and it has a prefix name
        if (itemRarity != null && itemRarity.contentPrefixName != string.Empty)
        {
            // add rarity's content prefix name to the full name
            fullItemName += (itemRarity.contentPrefixName + " ");
        }

        // if item has a prefix name
        if (itemPrefixName != string.Empty)
        {
            // add item's prefix name to the full name
            fullItemName += (itemPrefixName + " ");
        }

        // add item's base name to full name
        fullItemName += itemName;

        // return setup full name
        return fullItemName;
    }

    #endregion


}
