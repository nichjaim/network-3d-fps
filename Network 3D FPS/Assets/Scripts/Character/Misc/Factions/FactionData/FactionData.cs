using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FactionData
{
    #region Class Variables

    public string factionId;
    public string factionName;
    [TextArea(3, 15)]
    public string factionDescription;

    [Tooltip("Can their attacks hurt their own people")]
    public bool canFriendlyFire;
    [Tooltip("Will they attack factions they are unfamiliar with")]
    public bool inherentlyHostile;

    #endregion




    #region Constructors

    public FactionData()
    {
        Setup();
    }

    public FactionData(FactionData templateArg)
    {
        Setup(templateArg);
    }

    public FactionData(FactionDataTemplate templateArg)
    {
        Setup(templateArg.template);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        factionId = "0";
        factionName = "NAME";
        factionDescription = "DESCRIPTION";

        canFriendlyFire = false;
        inherentlyHostile = false;
    }

    private void Setup(FactionData templateArg)
    {
        factionId = templateArg.factionId;
        factionName = templateArg.factionName;
        factionDescription = templateArg.factionDescription;

        canFriendlyFire = templateArg.canFriendlyFire;
        inherentlyHostile = templateArg.inherentlyHostile;
    }

    #endregion


}
