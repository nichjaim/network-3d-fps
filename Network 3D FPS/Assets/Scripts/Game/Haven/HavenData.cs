using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data relevant to the player's party (group) home base of operations.
/// </summary>
[System.Serializable]
public class HavenData
{
    #region Class Variables

    // resources scaveneged in FPS part of game, this resource called 'Supplies' in-game
    public int externalResource = 0;

    public HavenResourcesCollection havenResources = null;

    /// this keeps track of how many social interaction/hangout time slot activities 
    /// that the player has done that did NOT involve the associated girl.
    /// This is needed to know when a girl has become eligble for a hangout bonus 
    /// (a mechanic to incentivize the player to get to know more than one girl).
    public HavenSocialDeprivation socialDeprivation = null;

    public TimeCalendarSystem calendarSystem = null;

    #endregion




    #region Constructors

    public HavenData()
    {
        Setup();
    }

    public HavenData(HavenData templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        externalResource = 0;

        havenResources = new HavenResourcesCollection();

        socialDeprivation = new HavenSocialDeprivation();

        calendarSystem = new TimeCalendarSystem();
    }

    private void Setup(HavenData templateArg)
    {
        externalResource = templateArg.externalResource;

        havenResources = new HavenResourcesCollection(templateArg.havenResources);

        socialDeprivation = new HavenSocialDeprivation(templateArg.socialDeprivation);

        calendarSystem = new TimeCalendarSystem(templateArg.calendarSystem);
    }

    #endregion


}
