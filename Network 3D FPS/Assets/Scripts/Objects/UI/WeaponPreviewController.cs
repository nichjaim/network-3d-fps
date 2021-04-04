using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPreviewController : MonoBehaviour
{
    #region Class Variables

    [SerializeField]
    private TextMeshPro textItemName = null;

    [Header("Damage Text")]

    [SerializeField]
    private TextMeshPro textWeaponDamageLow = null;
    [SerializeField]
    private TextMeshPro textWeaponDamageHigh = null;

    [Header("Attack Rate Text")]

    [SerializeField]
    private TextMeshPro textWeaponAttackRate = null;

    #endregion




    #region Preview Functions

    /// <summary>
    /// Sets up the preview info based on given item.
    /// </summary>
    /// <param name="templateArg"></param>
    public void SetupPreview(WeaponData templateArg)
    {
        // set name text to item's full name
        textItemName.text = templateArg.itemInfo.GetFullItemName();
        // set name text to item's rarity color
        textItemName.color = GeneralMethods.GetColorFromHexColorCode(
            templateArg.itemInfo.itemRarity.rarityHexColorCode);

        // set damage text to weapon's damage stats
        textWeaponDamageLow.text = templateArg.weaponStats.
            damageBoundaryLowRarityModified.ToString();
        textWeaponDamageHigh.text = templateArg.weaponStats.
            damageBoundaryHighRarityModified.ToString();

        // set atk rate text to weapon's atk rate stats
        textWeaponAttackRate.text = templateArg.weaponStats.
            attackRateRarityModified.ToString();
    }

    #endregion


}
