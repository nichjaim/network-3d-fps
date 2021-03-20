using UnityEngine;

[System.Serializable]
public class RarityTier
{
    #region Class Variables

    public string rarityId;
    public string rarityName;
    [TextArea(3, 15)]
    public string rarityDescription;

    public float statMultiplierBoundaryLow;
    public float statMultiplierBoundaryHigh;

    [HideInInspector]
    public string rarityHexColorCode;

    // the prefix name that the rarity can add to the associated content.
    [HideInInspector]
    public string contentPrefixName;

    #endregion




    #region Constructors

    public RarityTier()
    {
        Setup();
    }

    public RarityTier(RarityTier templateArg)
    {
        Setup(templateArg);
    }

    public RarityTier(RarityTierTemplate templateArg)
    {
        Setup(templateArg.template);

        rarityHexColorCode = ColorUtility.ToHtmlStringRGB(templateArg.rarityColor);

        SetupRarityPrefixName(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        rarityId = "0";
        rarityName = "NAME";
        rarityDescription = "DESCRIPTION";

        statMultiplierBoundaryLow = 1f;
        statMultiplierBoundaryHigh = 1f;

        rarityHexColorCode = "FFFFFF";

        contentPrefixName = "RARITY-PREFIX-NAME";
    }

    private void Setup(RarityTier templateArg)
    {
        rarityId = templateArg.rarityId;
        rarityName = templateArg.rarityName;
        rarityDescription = templateArg.rarityDescription;

        statMultiplierBoundaryLow = templateArg.statMultiplierBoundaryLow;
        statMultiplierBoundaryHigh = templateArg.statMultiplierBoundaryHigh;

        rarityHexColorCode = templateArg.rarityHexColorCode;

        contentPrefixName = templateArg.contentPrefixName;
    }

    private void SetupRarityPrefixName(RarityTierTemplate templateArg)
    {
        contentPrefixName = GeneralMethods.GetRandomStringListEntry(templateArg.potentialContentPrefixNames);
    }

    #endregion


}
